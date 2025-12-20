namespace Amazon.ProductCatalog.Domain.Products.ValueObjects;

public class ProductPriceChange
{
    public decimal From { get; private set; }
    public decimal To { get; private set; }
    public DateTime At { get; private set; }

    public ProductPriceChange(decimal from, decimal to, DateTime at)
    {
        From = from;
        To = to;
        At = at;
    }
}