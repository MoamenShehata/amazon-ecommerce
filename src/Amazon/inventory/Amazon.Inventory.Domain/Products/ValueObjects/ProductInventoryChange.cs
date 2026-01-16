using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Inventory.Domain.Products.ValueObjects;

public class ProductInventoryChange : IdentifiedValue<int>
{
    public Guid ProductId { get; private set; }
    public int Before { get; private set; }
    public int After { get; private set; }
    public DateTime OccurredOn { get; private set; }
    public ProductInventoryChange(Guid productId, int before, int after)
    {
        ProductId = productId;
        Before = before;
        After = after;
        OccurredOn = DateTime.UtcNow;
    }
}