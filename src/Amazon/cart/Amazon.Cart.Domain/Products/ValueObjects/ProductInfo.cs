namespace Amazon.Cart.Domain.Products.ValueObjects;

public class ProductInfo
{
    public string Name { get; private set; }
    public string ImageUrl { get; private set; }
    public decimal UnitPrice { get; private set; }

    public ProductInfo(string name, string imageUrl, decimal unitPrice)
    {
        Name = name;
        ImageUrl = imageUrl;
        UnitPrice = unitPrice;
    }

    public ProductInfo WithName(string name) => new ProductInfo(name, ImageUrl, UnitPrice);
    public ProductInfo WithNewPrice(decimal newPrice) => new ProductInfo(Name, ImageUrl, newPrice);

    #region Infra

    private ProductInfo()
    {

    }
    #endregion
}