using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Cart.Domain.Entities;

public class CartItem : Entity<Guid>, IEntity<Guid>
{
    public Guid ShoppingCartId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    internal CartItem(Guid shoppingCartId, Guid productId, int quantity) : base(Guid.NewGuid())
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        Quantity = quantity;
    }

    private CartItem() : base(Guid.Empty) { }
}