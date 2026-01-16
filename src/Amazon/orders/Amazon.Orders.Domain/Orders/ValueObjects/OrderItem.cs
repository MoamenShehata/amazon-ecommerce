using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Orders.Domain.Orders.ValueObjects;

public class OrderItem : IdentifiedValue<int>
{
    public Guid OrderId { get; private set; }
    public ProductInfo ProductInfo { get; private set; }
    public int Quantity { get; private set; }

    internal OrderItem(Guid orderId, ProductInfo productInfo, int quantity)
    {
        OrderId = orderId;
        ProductInfo = productInfo;
        Quantity = quantity;
    }

    public decimal Price => Quantity * ProductInfo.UnitPrice;

    #region Infra
    private OrderItem() { }
    #endregion
}
