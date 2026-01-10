using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.ProductCatalog.Domain.Categories.Events;

public class CategoryCreatedEvent : DomainEventBase
{
    public CategoryCreatedEvent(Guid categoryId) : base(DateTime.UtcNow)
    {
        CategoryId = categoryId;
    }

    public Guid CategoryId { get; }
}
