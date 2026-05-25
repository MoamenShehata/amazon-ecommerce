namespace Amazon.Cart.Domain.Services;

public interface IInventoryService
{
    Task<bool> IsProductAvailableForQuantityAsync(Guid productId, int quantity);
    Task<int> TryHoldProductItemForPurchaseAsync(Guid productId);
}
