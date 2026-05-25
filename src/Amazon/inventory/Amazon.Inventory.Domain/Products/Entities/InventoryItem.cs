using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Inventory.Domain.Products.Entities;

public class InventoryItem : AuditableEntity<int>
{
    public Guid ProductId { get; private set; }
    public InventoryItem(Guid productId) : base(0) => ProductId = productId;

    public Guid? ReservedForOrder { get; private set; }
    public void ReserveForOrder(Guid orderId) => ReservedForOrder = orderId;
    public void Release() => ReservedForOrder = null;

    public bool IsAvailable => ReservedForOrder is null;

}