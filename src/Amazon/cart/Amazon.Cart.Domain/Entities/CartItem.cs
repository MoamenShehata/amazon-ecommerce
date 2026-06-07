using Amazon.Cart.Domain.Products;

namespace Amazon.Cart.Domain.Entities;

public class CartItem
{
    public Guid ProductId { get; private set; }

    internal CartItem(Guid productId)
    {
        ProductId = productId;
    }

    #region Infra
    private CartItem() { }
    #endregion
}