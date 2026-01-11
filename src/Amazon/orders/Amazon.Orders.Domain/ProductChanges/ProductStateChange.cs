using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Orders.Domain.ProductChanges;

public class ProductStateChange : IdentifiedValue<int>
{
    public DateTime OccurredOn { get; private set; }
    public Guid ProductId { get; private set; }
    public int? InStockCount { get; private set; }
    public decimal? CurrentPrice { get; private set; }

    public ProductStateChange(DateTime occurredOn, Guid productId, int? inStockCount, decimal? currentPrice)
    {
        OccurredOn = occurredOn;
        ProductId = productId;
        InStockCount = inStockCount;
        CurrentPrice = currentPrice;
    }
}
