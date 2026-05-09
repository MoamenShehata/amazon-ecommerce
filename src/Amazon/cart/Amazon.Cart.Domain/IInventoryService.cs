
namespace Amazon.Cart.Domain;

public interface IInventoryService
{
    Task<int> TryHoldProductItemForPurchaseAsync(Guid productId);
}