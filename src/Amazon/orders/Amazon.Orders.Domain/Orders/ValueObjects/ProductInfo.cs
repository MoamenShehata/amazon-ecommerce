using System.Xml;

namespace Amazon.Orders.Domain.Orders.ValueObjects;

public record ProductInfo
{
    public Guid ProductId { get; init; }
    public decimal UnitPrice { get; init; }
    public string Name { get; init; }

    public ProductInfo(Guid productId, decimal unitPrice, string name)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(unitPrice, 0, nameof(unitPrice));

        ProductId = productId;
        UnitPrice = unitPrice;
        Name = name;
    }

    public ProductInfo WithPrice(decimal newPrice) => new(ProductId, newPrice, Name);

    private ProductInfo() { }
}
