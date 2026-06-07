using Amazon.Cart.Domain.Products;

namespace Amazon.Cart.Domain.Entities;

public class CartItem
{
    public Guid ProductId { get; private set; }
    public Product? Product { get; private set; }

    internal CartItem(Guid productId)
    {
        ProductId = productId;
    }

    public decimal Price => Product?.Info.UnitPrice ?? 0;

    #region Infra
    private CartItem() { }
    #endregion
}