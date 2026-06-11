namespace Amazon.Cart.Domain.ShoppingCarts.ValueObjects;

public class CartItemQuantity
{
    public int Quantity { get; private set; }

    public CartItemQuantity(int quantity)
    {
        Quantity = quantity;
    }

    public static implicit operator int(CartItemQuantity quantity) => quantity.Quantity;
}