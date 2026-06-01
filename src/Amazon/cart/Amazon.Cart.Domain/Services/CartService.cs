using Amazon.Cart.Domain.Entities;
using Amazon.Cart.Domain.Factories;
using Amazon.Cart.Domain.Integrations;
using Amazon.Cart.Domain.Payments;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
using Moamen.SDKs.Repository;

namespace Amazon.Cart.Domain.Services;

public class CartService(
        ShoppingCartFactory _cartFactory,
        IRepository<ShoppingCart, Guid> _cartsRepo,
        IInventoryService _inventoryService,
        ICustomerService _customerService,
        IOrderService _orderService,
        PaymentsService _paymentsService
        )
{
    public async Task<RestResponse<ShoppingCart>> CreateCartAsync(Guid? customerId)
    {
        var existingCustomerCart = await _cartsRepo.GetInstanceAsync(x => x.CustomerId == customerId && x.Expiration.ExpiresAt > DateTime.UtcNow);
        if (existingCustomerCart != null)
            return RestResponse<ShoppingCart>.Conflict($"Customer already has an active shopping cart");

        var cart = _cartFactory.Create(customerId);
        _cartsRepo.Add(cart);

        return RestResponse<ShoppingCart>.Created(cart, cart.Id.ToString());
    }

    public async Task<RestResponse<ShoppingCart>> GetByIdAsync(Guid cartId)
    {
        var cart = await _cartsRepo.GetInstanceAsync(x => x.Id == cartId);
        if (cart is null)
            return RestResponse<ShoppingCart>.NotFound($"Cart with id {cartId} was not found");

        return RestResponse<ShoppingCart>.Success(cart);
    }

    public async Task<RestResponse<ShoppingCart>> GetByIdForUserAsync(Guid cartId, Guid customerId)
    {
        var cart = await _cartsRepo.GetInstanceAsync(x => x.Id == cartId && x.CustomerId == customerId);
        if (cart is null)
            return RestResponse<ShoppingCart>.NotFound($"Cart with id {cartId} was not found");

        return RestResponse<ShoppingCart>.Success(cart);
    }

    public async Task<RestResponse<CartItem>> TryAddItemToCartAsync(ShoppingCart cart, Guid productId)
    {
        var totalItemsCountRequested = cart.GetItemsCountForProduct(productId) + 1;
        var isAvailable = await _inventoryService.IsProductAvailableForQuantityAsync(productId, totalItemsCountRequested);
        if (!isAvailable)
            return RestResponse<CartItem>.Conflict($"Product with id {productId} is not available in inventory");

        var item = cart.AddItem(productId);
        return RestResponse<CartItem>.Success(item);
    }

    public async Task<RestResponse<int>> SetupForCheckoutAsync(ShoppingCart cart, Guid userId, int deliverToAddressId, Guid paymentMethodId)
    {
        var attachResult = cart.AttachToUser(userId);
        if (!attachResult.IsSuccess)
            return attachResult.MapTo(-1);

        var deliveryAddressResult = await _customerService.GetCustomerDeliveryAddressOrDefaultAsync(userId, deliverToAddressId);
        if (!deliveryAddressResult.IsSuccess)
            return deliveryAddressResult.MapTo(-1);

        cart.SetDeliverToAddress(deliverToAddressId);

        return await _paymentsService.UsePaymentMethodAsync(paymentMethodId, userId, deliveryAddressResult);
    }

    public async Task<RestResponse<Guid>> TryCheckoutAsync(Guid cartId, Guid userId)
    {
        var cartResult = await GetByIdForUserAsync(cartId, userId);
        if (!cartResult.IsSuccess)
            return cartResult.MapTo(Guid.Empty);

        var orderAvailabilityResult = await CanOrderBeSatisifiedForCartAsync(cartResult);
        if (!orderAvailabilityResult.IsSuccess)
            return orderAvailabilityResult.MapTo(Guid.Empty);

        var orderIdCreated = await _orderService.CreateOrderAsync(userId, "should be queried from CART", [.. cartResult.Value.Items.GroupBy(x => x.ProductId).Select(x => new KeyValuePair<Guid, int>(x.Key, x.Count()))]);

        _cartsRepo.Remove(cartResult.Value);

        return RestResponse<Guid>.Success(orderIdCreated);
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