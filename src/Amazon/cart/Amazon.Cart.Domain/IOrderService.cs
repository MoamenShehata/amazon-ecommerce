
namespace Amazon.Cart.Domain;

public interface IOrderService
{
    Task<Guid> CreateOrderAsync(Guid UserId, string Email, List<KeyValuePair<Guid, int>> ShoppingCart);
}