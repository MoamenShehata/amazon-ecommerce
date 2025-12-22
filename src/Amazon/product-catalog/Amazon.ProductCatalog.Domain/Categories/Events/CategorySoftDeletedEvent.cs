using EMP.SharedKernel.DDD.Definitions;

namespace Amazon.ProductCatalog.Domain.Categories.Events;

public class CategorySoftDeletedEvent : DomainEvent
{
    public CategorySoftDeletedEvent(Guid categoryId, Guid? orphanProductsNewCategoryId) : base(DateTime.UtcNow, false)
    {
        CategoryId = categoryId;
        OrphanProductsNewCategoryId = orphanProductsNewCategoryId;
    }

    public Guid CategoryId { get; }
    public Guid? OrphanProductsNewCategoryId { get; }
}