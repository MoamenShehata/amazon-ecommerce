namespace Amazon.Cart.Domain.Services;

public interface IOrderService
{
    Task<Guid> CreateOrderAsync(Guid UserId, string Email, List<KeyValuePair<Guid, int>> ShoppingCart);
}