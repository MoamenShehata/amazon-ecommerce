using Amazon.Cart.Domain.Factories;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;

namespace Amazon.Cart.Domain;

public class ShoppingCartService(
        ShoppingCartFactory _cartFactory,
        IRepository<ShoppingCart, Guid> _cartsRepo
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
}