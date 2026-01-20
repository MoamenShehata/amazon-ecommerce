using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Cart.Domain.Entities;

public class CartItem : Entity<Guid>, IEntity<Guid>
{
    public Guid ShoppingCartId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public string ProductName { get; private set; }
    public string ProductImageUrl { get; private set; }

    internal CartItem(Guid shoppingCartId, Guid productId, int quantity, string productName, string productImageUrl) : base(Guid.NewGuid())
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        Quantity = quantity;
        ProductName = productName;
        ProductImageUrl = productImageUrl;
    }

    private CartItem() : base(Guid.Empty) { }
}