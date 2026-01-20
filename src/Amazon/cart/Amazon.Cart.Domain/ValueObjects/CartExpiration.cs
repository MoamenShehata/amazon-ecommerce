namespace Amazon.Cart.Domain.ValueObjects;

public class CartExpiration
{
    public DateTime ExpiresAt { get; private set; }

    public CartExpiration(DateTime expiresAt)
    {
        ExpiresAt = expiresAt.ToUniversalTime();
    }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
}