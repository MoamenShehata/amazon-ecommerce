using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.ProductCatalog.Domain.Products.Events;

public class ProductCreatedEvent : DomainEventBase
{
    public ProductCreatedEvent(Guid categoryId, Guid productId, string name, int inStockCount, decimal unitPrice) : base(DateTime.UtcNow, true)
    {
        CategoryId = categoryId;
        ProductId = productId;
        Name = name;
        InStockCount = inStockCount;
        UnitPrice = unitPrice;
    }

    public Guid CategoryId { get; }
    public Guid ProductId { get; }
    public string Name { get; }
    public int InStockCount { get; }
    public decimal UnitPrice { get; }
}