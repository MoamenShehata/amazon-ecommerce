using Amazon.ProductCatalog.Domain.Products.Events;
using Amazon.ProductCatalog.Domain.Products.ValueObjects;
using Amazon.SharedKernel.Common;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.ProductCatalog.Domain.Products;

public class Product : AuditableAggregate<Guid>, IEntity<Guid>
{
    public string Name { get; private set; }

    private HashSet<ProductProperty> _properties { get; set; } = new(new ProductPropertyComparer());
    public List<ProductProperty> Properties { get; private set; }
    //public ApiResult<bool> TryAddProperty(string name, string value)
    //{
    //    var result = _properties.Add(new ProductProperty(name, value));
    //    if (!result) return ApiResponseExtentions.Error<bool>($"Property with the name {name} already added on Product {Name}");

    //    return ApiResponseExtentions.Success(result);
    //}
    //public void RemoveProperty(string name) => _properties.RemoveWhere(x => x.Name.ToLower() == name.ToLower());

    public ProductPrice Price { get; private set; }
    private ICollection<ProductPriceChange> _priceChanges { get; } = new List<ProductPriceChange>();
    public void UpdatePrice(decimal newPrice)
    {
        if (Price == newPrice) return;

        _priceChanges.Add(new(Price, newPrice, DateTime.UtcNow));
        Price = Price.WithNew(newPrice);
    }

    public bool IsDeleted { get; private set; }
    public void SoftDelete() => IsDeleted = true;

    public Result<bool> UpdateFrom(string newName, decimal productPrice, List<ProductProperty> properties)
    {
        Name = newName;

        UpdatePrice(productPrice);

        Properties = properties;

        return Result<bool>.Success(true);
    }

    public void AttachToCategory(Guid categoryId) => CategoryId = categoryId;

    public Guid CategoryId { get; private set; }

    internal Product(Guid categoryId, string name, ProductPrice price, List<ProductProperty> properties) : base(Guid.NewGuid())
    {
        CategoryId = categoryId;
        Name = name;
        Price = price;

        Properties = properties;

        RaiseEvent(new ProductCreatedEvent(categoryId, Id, Name, Price.Amount));
    }

    private Product() : base(Guid.Empty) { }
}
