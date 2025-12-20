using System.Diagnostics.CodeAnalysis;

namespace Amazon.ProductCatalog.Domain.Products.ValueObjects;

public class ProductProperty
{
    public string Name { get; private set; }
    public string Value { get; private set; }

    public ProductProperty(string name, string value)
    {
        Name = name;
        Value = value;
    }
}

public class ProductPropertyComparer : IEqualityComparer<ProductProperty>
{
    public bool Equals(ProductProperty? x, ProductProperty? y) => x.Name.ToLower() == y.Name.ToLower();
    public int GetHashCode([DisallowNull] ProductProperty obj) => obj.Name.ToLower().GetHashCode();
}