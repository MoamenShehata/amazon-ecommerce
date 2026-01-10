using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.ProductCatalog.Domain.Products.Events;

public class ProductCreatedEvent : DomainEventBase
{
    public ProductCreatedEvent(Guid categoryId, Guid productId) : base(DateTime.UtcNow)
    {
        CategoryId = categoryId;
        ProductId = productId;
    }

    public Guid CategoryId { get; }
    public Guid ProductId { get; }
}