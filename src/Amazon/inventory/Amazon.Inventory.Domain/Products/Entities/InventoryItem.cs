using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Inventory.Domain.Products.Entities;

public class InventoryItem : AuditableEntity<int>
{
    public Guid ProductId { get; private set; }
    public bool IsOnHold { get; private set; }
    public InventoryItem(Guid productId) : base(0) => ProductId = productId;

    public void HoldForPurchase() => IsOnHold = true;
    public void ReleaseHold() => IsOnHold = false;
}