using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.ProductCatalog.Domain.Categories.Events;

public class CategorySoftDeletedEvent : DomainEventBase
{
    public CategorySoftDeletedEvent(Guid categoryId, Guid? orphanProductsNewCategoryId) : base(DateTime.UtcNow)
    {
        CategoryId = categoryId;
        OrphanProductsNewCategoryId = orphanProductsNewCategoryId;
    }

    public Guid CategoryId { get; }
    public Guid? OrphanProductsNewCategoryId { get; }
}