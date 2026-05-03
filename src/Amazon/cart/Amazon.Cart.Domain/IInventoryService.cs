
namespace Amazon.Cart.Domain;

public interface IInventoryService
{
    Task<bool> IsProductAvailableAsync(Guid productId);
}