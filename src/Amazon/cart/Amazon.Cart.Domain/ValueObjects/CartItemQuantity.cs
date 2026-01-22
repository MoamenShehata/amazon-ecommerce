namespace Amazon.Cart.Domain.ValueObjects;

public class CartItemQuantity
{
    private const int _incrementBy = 1;

    public int Quantity { get; private set; }
    private CartItemQuantity(int quantity) => Quantity = quantity;
    internal CartItemQuantity() : this(1) { }

    public CartItemQuantity Increment() => new(Quantity + _incrementBy);
    public CartItemQuantity Decrement() => new(Quantity - _incrementBy);
    public bool IsZero => Quantity == 0;
}