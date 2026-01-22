using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Cart.Domain.Entities;

public class CartItem : IdentifiedValue<int>
{
    public Guid ShoppingCartId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public string ProductImageUrl { get; private set; }

    internal CartItem(Guid shoppingCartId, Guid productId, string productName, string productImageUrl)
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        ProductName = productName;
        ProductImageUrl = productImageUrl;
    }


    #region Infra
    private CartItem() { }
    #endregion
}