using Amazon.Cart.Domain.Entities;
using Amazon.Cart.Domain.Factories;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;

namespace Amazon.Cart.Domain;

public class ShoppingCartService(
        ShoppingCartFactory _cartFactory,
        IRepository<ShoppingCart, Guid> _cartsRepo,
        IInventoryService _inventoryService
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

    public async Task<RestResponse<CartItem>> TryAddItemToCartAsync(ShoppingCart cart, Guid productId, string productName, string productImageUrl)
    {
        var totalItemsCountRequested = cart.GetItemsCountForProduct(productId) + 1;
        var isAvailable = await _inventoryService.IsProductAvailableForQuantityAsync(productId, totalItemsCountRequested);
        if (!isAvailable)
            return RestResponse<CartItem>.Conflict($"Product with id {productId} is not available in inventory");

        var item = cart.AddItem(productId, productName, productImageUrl);
        return RestResponse<CartItem>.Success(item);
    }
}