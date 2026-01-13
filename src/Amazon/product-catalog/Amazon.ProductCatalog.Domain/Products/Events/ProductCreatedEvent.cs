using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.ProductCatalog.Domain.Products.Events;

public class ProductCreatedEvent : DomainEventBase
{
    public ProductCreatedEvent(Guid categoryId, Guid productId, string Name, decimal UnitPrice) : base(DateTime.UtcNow, true)
    {
        CategoryId = categoryId;
        ProductId = productId;
        this.Name = Name;
        this.UnitPrice = UnitPrice;
    }

    public Guid CategoryId { get; }
    public Guid ProductId { get; }
    public string Name { get; }
    public decimal UnitPrice { get; }
}