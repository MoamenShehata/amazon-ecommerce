using Amazon.ProductCatalog.Domain.Categories.Events;
using Amazon.ProductCatalog.Domain.Products;
using Amazon.ProductCatalog.Domain.Products.ValueObjects;
using EMP.SharedKernel.DDD.Definitions;

namespace Amazon.ProductCatalog.Domain.Categories;

public class Category : AuditableAggregate<Guid>
{
    public string Name { get; private set; }

    public bool IsDeleted { get; private set; }
    public void SoftDelete(Guid? orphanProductsNewCategoryId)
    {
        if (IsDeleted) return;

        IsDeleted = true;
        RaiseEvent(new CategorySoftDeletedEvent(Id, orphanProductsNewCategoryId));
    }

    public Guid? ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }

    internal Category(string name) : base(Guid.NewGuid())
    {
        Name = name;

        RaiseEvent(new CategoryCreatedEvent(Id));
    }

    public Product NewProduct(string name, ProductPrice price) => new(Id, name, price);
}