using Amazon.Cart.Domain.Products;
using Amazon.Cart.Domain.Products.ValueObjects;
using Amazon.Cart.Domain.ShoppingCarts.ValueObjects;

namespace Amazon.Cart.Domain.ShoppingCarts.Entites;

public class CartItem
{
    public Guid ProductId { get; private set; }
    public ProductInfo Info { get; private set; }

    public CartItemQuantity Quantity { get; private set; }
    internal void IncrementByOne() => Quantity = new CartItemQuantity(Quantity + 1);
    internal void DecrementByOne() => Quantity = new CartItemQuantity(Math.Max(Quantity - 1, 0));

    internal CartItem(Product product)
    {
        ProductId = product.Id;
        Info = product.Info;

        Quantity = new CartItemQuantity(1);
    }

    public decimal UnitPrice => Info.UnitPrice;
    public decimal TotalPrice => Quantity * Info.UnitPrice;

    #region Infra
    private CartItem() { }
    #endregion
}