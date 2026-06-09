using Amazon.Cart.Domain.Factories;
using Amazon.Cart.Domain.Integrations.Customers;
using Amazon.Cart.Domain.Integrations.Customers.Dtos;
using Amazon.Cart.Domain.Integrations.Orders;
using Amazon.Cart.Domain.Integrations.Orders.Dtos;
using Amazon.Cart.Domain.Payments;
using Amazon.Cart.Domain.Products;
using Amazon.Cart.Domain.Specifications;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Logging;
using Moamen.SDKs.Repository;

namespace Amazon.Cart.Domain.Services;

public class CartService(
        ShoppingCartFactory _cartFactory,
        IRepository<ShoppingCart, Guid> _carts,
        IRepository<Product, Guid> _products,
        IInventoryService _inventoryService,
        ICustomersIntegration _customerIntegration,
        IOrdersIntegration _ordersIntegration,
        PaymentsService _paymentsService,
        ILogger<CartService> _logger,
        ShoppingCartSpecification _specification,
        ProductService _productService
        )
{
    public async Task<RestResponse<ShoppingCart>> CreateCartAsync(Guid? customerId)
    {
        var existingCustomerCart = await _carts.GetInstanceAsync(x => x.CustomerId == customerId && x.Expiration.ExpiresAt > DateTime.UtcNow);
        if (existingCustomerCart != null)
            return RestResponse<ShoppingCart>.Conflict($"Customer already has an active shopping cart");

        var cart = _cartFactory.Create(customerId);
        _carts.Add(cart);

        return RestResponse<ShoppingCart>.Created(cart, cart.Id.ToString());
    }

    public async Task<RestResponse<ShoppingCart>> GetByIdAsync(Guid cartId)
    {
        var cart = await _carts.GetInstanceAsync(x => x.Id == cartId);
        if (cart is null)
            return RestResponse<ShoppingCart>.NotFound($"Cart with id {cartId} was not found");

        return RestResponse<ShoppingCart>.Success(cart);
    }

    public async Task<RestResponse<ShoppingCart>> GetForCheckoutAsync(Guid cartId)
    {
        var cart = await _carts.GetInstanceAsync(x => x.Id == cartId);
        if (cart is null)
            return RestResponse<ShoppingCart>.NotFound($"Cart with id {cartId} was not found");

        var isCartStateSatisfied = await _specification.SatisfiesAsync(cart);
        if (!isCartStateSatisfied.IsSuccess)
            return isCartStateSatisfied.MapTo(null as ShoppingCart);

        return RestResponse<ShoppingCart>.Success(cart);
    }

    public async Task<RestResponse<ShoppingCart>> GetByIdForUserAsync(Guid cartId, Guid customerId)
    {
        var cart = await _carts.GetInstanceAsync(x => x.Id == cartId && x.CustomerId == customerId);
        if (cart is null)
            return RestResponse<ShoppingCart>.NotFound($"Cart with id {cartId} was not found");

        return RestResponse<ShoppingCart>.Success(cart);
    }

    public async Task<RestResponse<bool>> TryAddItemToCartAsync(ShoppingCart cart, Guid productId)
    {
        var addResult = await _productService.CreateCartItemAsync(cart, productId);
        var product = await _products.GetInstanceAsync(productId);
        if (!addResult.IsSuccess)
            return addResult;

        var totalItemsCountRequested = cart.GetItemsCountForProduct(productId) + 1;
        var isAvailable = await _inventoryService.IsProductAvailableForQuantityAsync(productId, totalItemsCountRequested);
        if (!isAvailable)
            return RestResponse<bool>.Conflict($"Product with id {productId} is not available in inventory");

        return RestResponse<bool>.Success(true);
    }

    public async Task<RestResponse<int>> SetupForCheckoutAsync(Guid cartId, Guid userId, int deliverToAddressId, Guid paymentMethodId)
    {
        var cartResult = await GetByIdAsync(cartId);
        if (!cartResult.IsSuccess)
            return cartResult.MapTo(-1);

        var isCartStateSatisfied = await _specification.SatisfiesAsync(cartResult);
        if (!isCartStateSatisfied.IsSuccess)
            return isCartStateSatisfied.MapTo(-1);

        var attachResult = cartResult.Value.AttachToUser(userId);
        if (!attachResult.IsSuccess)
            return attachResult.MapTo(-1);

        var deliveryAddressResult = await _customerIntegration.GetDeliveryAddressOrDefaultAsync(deliverToAddressId);
        if (!deliveryAddressResult.IsSuccess)
            return deliveryAddressResult.MapTo(-1);

        cartResult.Value.SetDeliverToAddress(deliverToAddressId);

        return await _paymentsService.UsePaymentMethodAsync(paymentMethodId, userId, deliveryAddressResult);
    }

    public async Task<RestResponse<Guid>> CreateOrderAsync(ShoppingCart cart, Guid userId)
    {
        var orderId = Guid.NewGuid();

        var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["correlationId"] = orderId,
        });

        var orderAvailabilityResult = await CanOrderBeSatisifiedForCartAsync(cart);
        if (!orderAvailabilityResult.IsSuccess)
            return orderAvailabilityResult.MapTo(Guid.Empty);

        var deliveryAddressResult = await _customerIntegration.GetDeliveryAddressOrDefaultAsync(cart.DeliverToAddressId);

        var createdOrder = await _ordersIntegration.CreateAsync(ConstructOrderRequest(orderId, cart, deliveryAddressResult));

        scope.Dispose();
        return RestResponse<Guid>.Success(createdOrder.Id);
    }

    private OrderCreateDto ConstructOrderRequest(Guid orderId, ShoppingCart cart, CustomerDeliveryAddress deliverToAddressInfo)
    {
        return new OrderCreateDto(orderId, cart.AggregatToProducts, deliverToAddressInfo.AsOrderDeliveryAddress);
    }

    private async Task<RestResponse<bool>> CanOrderBeSatisifiedForCartAsync(ShoppingCart cart)
    {
        var orderItemsQuantities = cart.Items.GroupBy(x => x.ProductId);
        foreach (var productItemsGroup in orderItemsQuantities)
        {
            var isProductAvailableForTotalQuantity = await _inventoryService.IsProductAvailableForQuantityAsync(productItemsGroup.Key, productItemsGroup.Count());
            if (!isProductAvailableForTotalQuantity)
                return RestResponse<bool>.NotFound($"Product with id {productItemsGroup.Key} is not available in inventory");
        }

        return RestResponse<bool>.Success(true);
    }
}