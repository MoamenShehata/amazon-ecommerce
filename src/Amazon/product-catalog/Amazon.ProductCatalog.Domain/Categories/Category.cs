using Amazon.ProductCatalog.Domain.Categories.Events;
using Amazon.ProductCatalog.Domain.Products;
using Amazon.ProductCatalog.Domain.Products.ValueObjects;
using EMP.SharedKernel.DDD.Definitions;

namespace Amazon.ProductCatalog.Domain.Categories;

public class Category : AuditableAggregate<Guid>
{
    public string Name { get; private set; }

    public Guid? ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }

    internal Category(string name, Category? newParentCategory) : base(Guid.NewGuid())
    {
        Name = name;

        SetParentCategory(newParentCategory);

        RaiseEvent(new CategoryCreatedEvent(Id));
    }

    private void SetParentCategory(Category? newParentCategory)
    {
        if (newParentCategory is null) return;

        if (newParentCategory.Id == Id || newParentCategory == this)
            throw new Exception("A category cannot be parent of itself.");

        ParentCategory = newParentCategory;
        ParentCategoryId = newParentCategory.Id;
    }

    public Product NewProduct(string name, ProductPrice price) => new(Id, name, price);

    public void Update(string name, Category? newParentCategory)
    {
        Name = name;

        SetParentCategory(newParentCategory);
    }

    public bool IsDeleted { get; private set; }
    public void SoftDelete(Guid? orphanProductsNewCategoryId)
    {
        if (IsDeleted) return;

        IsDeleted = true;
        RaiseEvent(new CategorySoftDeletedEvent(Id, orphanProductsNewCategoryId));
    }
}