using System.Xml;

namespace Amazon.Orders.Domain.Orders.ValueObjects;

public record ProductInstance
{
    public Guid ProductId { get; init; }
    public decimal UnitPrice { get; init; }

    public ProductInstance(Guid productId, decimal unitPrice)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(unitPrice, 0, nameof(unitPrice));

        ProductId = productId;
        UnitPrice = unitPrice;
    }

    public ProductInstance WithPrice(decimal newPrice) => new(ProductId, newPrice);

    public OrderItem CreateOrderItem(int quantity)
    {
        return new OrderItem(this, quantity);
    }

}
