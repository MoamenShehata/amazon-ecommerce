using Amazon.Cart.Domain.Entities;
using Amazon.Cart.Domain.ValueObjects;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Cart.Domain;

public class ShoppingCart : AuditableAggregate<Guid>, IEntity<Guid>
{
    public Guid? CustomerId { get; private set; }
    public CartExpiration Expiration { get; private set; }

    internal ShoppingCart(Guid? customerId, CartExpiration expiration) : base(Guid.NewGuid())
    {
        CustomerId = customerId;
        Expiration = expiration;
    }

    private readonly List<CartItem> _cartItems = new();
    public IReadOnlyCollection<CartItem> Items => _cartItems;


    public Guid AddItem(Guid productId, int quantity)
    {
        var item = new CartItem(Id, productId, quantity);
        _cartItems.Add(item);

        return item.Id;
    }

    public void RemoveItem(Guid itemId)
    {
        var item = _cartItems.FirstOrDefault(x => x.Id == itemId);
        if (item != null)
            _cartItems.Remove(item);
    }

    #region Infra
    private ShoppingCart() : this(Guid.NewGuid(), null)
    {
    }
    #endregion
}