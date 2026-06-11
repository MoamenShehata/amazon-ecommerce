using Amazon.Cart.Domain.Payments;
using Amazon.Cart.Domain.Products;
using Amazon.Cart.Domain.ShoppingCarts.Entites;
using Amazon.Cart.Domain.ValueObjects;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Cart.Domain.ShoppingCarts;

public class ShoppingCart : AuditableAggregate<Guid>, IEntity<Guid>
{
    public Guid? CustomerId { get; private set; }
    public Guid? OrderId { get; private set; }
    public RestResponse<bool> SetOrder(Guid orderId)
    {
        if (OrderId.HasValue)
            return RestResponse<bool>.BadRequest("You already tried to checkout this cart, please confirm your order");

        OrderId = orderId;
        return RestResponse<bool>.Success(true);
    }

    public PaymentMehodType? PaymentMethod { get; private set; }
    public void SetPaymentMethod(PaymentMehodType paymentMethod)
    {
        PaymentMethod = paymentMethod;
    }

    public string? CheckedoutSessionId { get; private set; }
    public RestResponse<bool> SetCheckedoutSession(string checkedoutSessionId)
    {
        if (!string.IsNullOrWhiteSpace(CheckedoutSessionId))
            return RestResponse<bool>.BadRequest("You already have an active checkout session");

        CheckedoutSessionId = checkedoutSessionId;
        return RestResponse<bool>.Success(true);
    }

    public CartExpiration Expiration { get; private set; }

    public int? DeliverToAddressId { get; private set; }

    internal ShoppingCart(Guid? customerId, CartExpiration expiration) : base(Guid.NewGuid())
    {
        CustomerId = customerId;
        Expiration = expiration;
    }

    private List<CartItem> _cartItems = new();
    public IReadOnlyCollection<CartItem> Items => _cartItems;
    public CartItem PushProductItem(Product product)
    {
        var existingProductItem = FindProductItem(product.Id);
        if (existingProductItem is not null)
        {
            existingProductItem.IncrementByOne();
            return existingProductItem;
        }

        var newCartItem = product.CreateCartItem();
        _cartItems.Add(newCartItem);
        return newCartItem;
    }

    public void PopProductItem(Guid productId)
    {
        var existingProductItem = FindProductItem(productId);
        if (existingProductItem is null) return;

        existingProductItem.DecrementByOne();

        if (existingProductItem.Quantity == 0)
            _cartItems.Remove(existingProductItem);
    }
    public void RemoveProductItems(Guid productId) => _cartItems.RemoveAll(x => x.ProductId == productId);

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

    public decimal TotalAmount => _cartItems.Sum(i => i.UnitPrice);

    public List<KeyValuePair<Guid, int>> AggregatToProducts => _cartItems.GroupBy(i => i.ProductId)
            .Select(g => new KeyValuePair<Guid, int>(g.Key, g.Count()))
            .ToList();

    private CartItem FindProductItem(Guid productId) => _cartItems.FirstOrDefault(x => x.ProductId == productId);

    #region Infra
    private ShoppingCart() : this(Guid.NewGuid(), null)
    {
    }
    #endregion
}