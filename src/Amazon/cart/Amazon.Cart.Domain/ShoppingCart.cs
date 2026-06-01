using Amazon.Cart.Domain.Entities;
using Amazon.Cart.Domain.ValueObjects;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Cart.Domain;

public class ShoppingCart : AuditableAggregate<Guid>, IEntity<Guid>
{
    public Guid? CustomerId { get; private set; }

    public CartExpiration Expiration { get; private set; }

    public int? DeliverToAddressId { get; private set; }

    internal ShoppingCart(Guid? customerId, CartExpiration expiration) : base(Guid.NewGuid())
    {
        CustomerId = customerId;
        Expiration = expiration;
    }

    private readonly List<CartItem> _cartItems = new();
    public IReadOnlyCollection<CartItem> Items => _cartItems;


    public CartItem AddItem(Guid productId)
    {
        var item = new CartItem(Id, productId);
        _cartItems.Add(item);
        return item;
    }

    public void RemoveItem(int itemId)
    {
        var cartItem = _cartItems.FirstOrDefault(x => x.Id == itemId);
        if (cartItem == null) return;

        _cartItems.Remove(cartItem);
    }

    public void RemoveProductItems(Guid productId)
    {
        _cartItems.RemoveAll(x => x.ProductId == productId);
    }

    public void SetDeliverToAddress(int addressId) => DeliverToAddressId = addressId;

    public RestResponse<bool> AttachToUser(Guid userId)
    {
        if (!CanBeCheckedoutForUser(userId))
            return RestResponse<bool>.BadRequest($"Cart is owned by another user!");

        CustomerId = userId;
        return RestResponse<bool>.Success(true);
    }

    private bool CanBeCheckedoutForUser(Guid userId) => !CustomerId.HasValue || CustomerId == userId;

    public int GetItemsCountForProduct(Guid productId) => _cartItems.Count(i => i.ProductId == productId);

    public void Clear() => _cartItems.Clear();

    #region Infra
    private ShoppingCart() : this(Guid.NewGuid(), null)
    {
    }
    #endregion
}