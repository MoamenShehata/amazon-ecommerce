using Amazon.Cart.Domain.Products;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Cart.Domain.Entities;

public class CartItem : IdentifiedValue<int>
{
    public Guid ShoppingCartId { get; private set; }

    public Guid ProductId { get; private set; }
    public Product? Product { get; private set; }

    internal CartItem(Guid shoppingCartId, Guid productId)
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
    }


    #region Infra
    private CartItem() { }
    #endregion
}