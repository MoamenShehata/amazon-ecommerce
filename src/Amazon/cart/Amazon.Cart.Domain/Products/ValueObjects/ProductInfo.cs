namespace Amazon.Cart.Domain.Products.ValueObjects;

public class ProductInfo
{
    public string Name { get; private set; }
    public string ImageUrl { get; private set; }

    public ProductInfo(string name, string imageUrl)
    {
        Name = name;
        ImageUrl = imageUrl;
    }

    public ProductInfo WithName(string name) => new ProductInfo(name, ImageUrl);

    #region Infra

    private ProductInfo()
    {
        
    }
    #endregion
}