using EMP.SharedKernel.DDD.Definitions;

namespace Amazon.ProductCatalog.Domain.Products.Events;

public class ProductCreatedEvent : DomainEvent
{
    public ProductCreatedEvent(Guid categoryId, Guid productId) : base(DateTime.UtcNow, false)
    {
        CategoryId = categoryId;
        ProductId = productId;
    }

    public Guid CategoryId { get; }
    public Guid ProductId { get; }
}