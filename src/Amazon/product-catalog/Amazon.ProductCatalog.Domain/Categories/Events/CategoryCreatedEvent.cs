using EMP.SharedKernel.DDD.Definitions;

namespace Amazon.ProductCatalog.Domain.Categories.Events;

public class CategoryCreatedEvent : DomainEvent
{
    public CategoryCreatedEvent(Guid categoryId) : base(DateTime.UtcNow, false)
    {
        CategoryId = categoryId;
    }

    public Guid CategoryId { get; }
}