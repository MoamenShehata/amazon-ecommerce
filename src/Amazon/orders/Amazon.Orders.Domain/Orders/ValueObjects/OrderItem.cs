using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Orders.Domain.Orders.ValueObjects;

public class OrderItem : IdentifiedValue<int>
{
    public ProductInstance ProductInfo { get; private set; }
    public int Quantity { get; private set; }

    internal OrderItem(ProductInstance productInfo, int quantity)
    {
        ProductInfo = productInfo;
        Quantity = quantity;
    }

    public decimal Price => Quantity * ProductInfo.UnitPrice;

    private OrderItem()
    {

    }
}
