using Amazon.Cart.Domain.Factories;
using Amazon.Cart.Domain.Integrations.Inventory;
using Amazon.Cart.Domain.Integrations.Orders;
using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.Cart.Domain.ShoppingCarts.Entites;
using Amazon.Cart.Domain.Specifications;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Moamen.SDKs.Repository;

namespace Amazon.Cart.Domain.Services;

public class CartService(
        ShoppingCartFactory _cartFactory,
        IRepository<ShoppingCart, Guid> _carts,
        IInventoryIntegration _inventoryService,
        IOrdersIntegration _ordersIntegration,
        ShoppingCartSpecification _specification,
        ProductService _productService
        )
{
    public async Task<ShoppingCart> EnsureCartExitsAsync(Guid? customerId)
    {
        var existingCustomerCart = await _carts.GetInstanceAsync(x => x.CustomerId == customerId && x.Expiration.ExpiresAt > DateTime.UtcNow);
        return existingCustomerCart ?? _cartFactory.Create(customerId);
    }

    public async Task<RestResponse<ShoppingCart>> GetByIdAsync(Guid cartId)
    {
        var cart = await _carts.GetInstanceAsync(x => x.Id == cartId && x.Expiration.ExpiresAt > DateTime.UtcNow);
        return cart is null
            ? RestResponse<ShoppingCart>.NotFound($"Cart with id {cartId} was not found")
            : RestResponse<ShoppingCart>.Success(cart);
    }

    public async Task<RestResponse<ShoppingCart>> GetForCheckoutAsync(Guid cartId)
    {
        var cart = await GetByIdAsync(cartId);
        if (!cart.IsSuccess) return cart;

        var isCartStateSatisfied = await _specification.SatisfiesAsync(cart);
        if (!isCartStateSatisfied.IsSuccess)
            return isCartStateSatisfied.MapTo(null as ShoppingCart);

        return RestResponse<ShoppingCart>.Success(cart);
    }
    
    public async Task<RestResponse<ShoppingCart>> GetForCheckoutConfimrationAsync(Guid cartId)
    {
        var cart = await GetByIdAsync(cartId);
        if (!cart.IsSuccess) return cart;

        var isCartStateSatisfied = await _specification.SatisfiesAsync(cart);
        if (!isCartStateSatisfied.IsSuccess)
            return isCartStateSatisfied.MapTo(null as ShoppingCart);

        if (!cart.Value.OrderId.HasValue || !cart.Value.PaymentMethod.HasValue)
            return RestResponse<ShoppingCart>.BadRequest("Cart has not been checed out  yet!");

        return RestResponse<ShoppingCart>.Success(cart);
    }

    public async Task<RestResponse<CartItem>> TryAddItemToCartAsync(ShoppingCart cart, Guid productId)
    {
        var addResult = await _productService.CreateCartItemAsync(cart, productId);
        if (!addResult.IsSuccess)
            return addResult.MapTo(null as CartItem);

        var availableResult = await _inventoryService.IsProductAvailableForQuantityAsync(productId, addResult.Value.Quantity);
        if (!availableResult.IsSuccess)
            return availableResult.MapTo(null as CartItem);

        return RestResponse<CartItem>.Success(addResult.Value);
    }

    public async Task<RestResponse<ShoppingCart>> EnsureCartHasOrderAsync(Guid cartId)
    {
        var shoppingCart = await GetForCheckoutAsync(cartId);
        if (!shoppingCart.IsSuccess)
            return shoppingCart.MapTo(null as ShoppingCart);

        if (shoppingCart.Value.OrderId.HasValue)
            return RestResponse<ShoppingCart>.Success(shoppingCart);

        var orderCreateResult = await _ordersIntegration.CreateAsync(shoppingCart);
        if (!orderCreateResult.IsSuccess)
            return orderCreateResult.MapTo(null as ShoppingCart);

        shoppingCart.Value.SetOrder(orderCreateResult.Value.Id);
        return RestResponse<ShoppingCart>.Success(shoppingCart);
    }


    //private async Task<RestResponse<bool>> CanOrderBeSatisifiedForCartAsync(ShoppingCart cart)
    //{
    //    var orderItemsQuantities = cart.Items.GroupBy(x => x.ProductId);
    //    foreach (var productItemsGroup in orderItemsQuantities)
    //    {
    //        var isProductAvailableForTotalQuantity = await _inventoryService.IsProductAvailableForQuantityAsync(productItemsGroup.Key, productItemsGroup.Count());
    //        if (!isProductAvailableForTotalQuantity)
    //            return RestResponse<bool>.NotFound($"Product with id {productItemsGroup.Key} is not available in inventory");
    //    }

    //    return RestResponse<bool>.Success(true);
    //}
    //public async Task<RestResponse<ShoppingCart>> GetByIdForUserAsync(Guid cartId, Guid customerId)
    //{
    //    var cart = await _carts.GetInstanceAsync(x => x.Id == cartId && x.CustomerId == customerId);
    //    if (cart is null)
    //        return RestResponse<ShoppingCart>.NotFound($"Cart with id {cartId} was not found");

    //    return RestResponse<ShoppingCart>.Success(cart);
    //}
}