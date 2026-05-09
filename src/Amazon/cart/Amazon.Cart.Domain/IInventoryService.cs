
namespace Amazon.Cart.Domain;

public interface IInventoryService
{
    Task<bool> IsProductAvailableForQuantityAsync(Guid productId, int quantity);
    Task<int> TryHoldProductItemForPurchaseAsync(Guid productId);
}