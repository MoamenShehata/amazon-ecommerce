using Amazon.ProductCatalog.Domain.Products;
using Amazon.ProductCatalog.Domain.Products.ValueObjects;
using Amazon.SharedKernel.Categories.Events;
using MassTransit.Middleware;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.ProductCatalog.Domain.Categories;

public class Category : AuditableAggregate<Guid>, IEntity<Guid>
{
    public string Name { get; private set; }
    public string FullName { get; private set; }

    public Guid? ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }

    private ICollection<Category> _children = [];
    public IReadOnlyCollection<Category> Children => _children.ToList();

    internal Category(string name, Category? newParentCategory) : base(Guid.NewGuid())
    {
        Name = name;

        SetParentCategory(newParentCategory);

        RaiseEvent(new CategoryCreatedEvent(Id));
    }

    private void SetParentCategory(Category? newParentCategory)
    {
        var parentsName = newParentCategory != null ? $",{newParentCategory.FullName}" : string.Empty;
        FullName = $"{Name}{parentsName}";

        if (newParentCategory is null) return;

        if (newParentCategory.Id == Id || newParentCategory == this)
            throw new Exception("A category cannot be parent of itself.");

        ParentCategory = newParentCategory;
        ParentCategoryId = newParentCategory.Id;
    }

    public Product NewProduct(string name, string imageUrl, ProductPrice price, List<ProductProperty> properties) => new(Id, name, price, imageUrl, properties);

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

    #region Infra
    private Category() : base(Guid.Empty) { }
    #endregion
}