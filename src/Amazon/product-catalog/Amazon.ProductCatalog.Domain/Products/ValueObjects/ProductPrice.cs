namespace Amazon.ProductCatalog.Domain.Products.ValueObjects;

public class ProductPrice
{
    public decimal Amount { get; private set; }
    public decimal Min { get; private set; }
    public decimal Max { get; private set; }

    public ProductPrice(decimal value, decimal min, decimal max)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, min);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, max);

        Amount = value;
        Min = min;
        Max = max;
    }
    public ProductPrice WithNew(decimal value) => new ProductPrice(value, Min, Max);

    public static implicit operator decimal(ProductPrice price) => price.Amount;
    private ProductPrice() { }
}