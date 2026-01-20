using Amazon.Cart.Domain.ValueObjects;

namespace Amazon.Cart.Domain.Factories;

public class ShoppingCartFactory
{
    public ShoppingCart Create(Guid? customerId)
    {
        // we can use customerId to get analysis on his previous data to create customer-specific expiration

        return new ShoppingCart(customerId, new CartExpiration(DateTime.UtcNow.AddMinutes(5)));
    }
}