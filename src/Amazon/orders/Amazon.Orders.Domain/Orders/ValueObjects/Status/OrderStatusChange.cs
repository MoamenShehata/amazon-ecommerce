using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Orders.Domain.Orders.ValueObjects.Status;

public abstract class OrderStatusChange : IdentifiedValue<int>
{
    public Guid OrderId { get; set; }
    public OrderState State { get; set; }
    public DateTime CreatedAt { get; set; }

    protected OrderStatusChange(Guid orderId, OrderState state)
    {
        State = state;
        CreatedAt = DateTime.UtcNow;
        OrderId = orderId;
    }

    public override string ToString() => $"{State} {CreatedAt}";
    public virtual object AdditionalInfo => string.Empty;

    private OrderStatusChange()
    {

    }
}

public class OrderCreatedStatus(Guid orderId) : OrderStatusChange(orderId, OrderState.Created)
{
    private OrderCreatedStatus() : this(Guid.Empty) { }
}

public class OrderCancelledStatus(Guid orderId) : OrderStatusChange(orderId, OrderState.Cancelled)
{
    private OrderCancelledStatus() : this(Guid.Empty) { }
}

public class OrderProcessingStatus(Guid orderId) : OrderStatusChange(orderId, OrderState.Processing)
{
    private OrderProcessingStatus() : this(Guid.Empty) { }
}

public class OrderShippingStartedStatus : OrderStatusChange
{
    public ShippingCompanyInfo CompanyInfo { get; private set; }
    public OrderShippingStartedStatus(Guid orderId, ShippingCompanyInfo companyInfo) : base(orderId, OrderState.ShippingStarted)
    {
        CompanyInfo = companyInfo;
    }

    public override object AdditionalInfo => CompanyInfo;

    private OrderShippingStartedStatus() : this(Guid.Empty, null) { }
}

public class OrderShippedStatus : OrderStatusChange
{
    public string TrackingId { get; private set; }
    public OrderShippedStatus(Guid orderId, string trackingId) : base(orderId, OrderState.Shipped)
    {
        TrackingId = trackingId;
    }

    public override object AdditionalInfo => TrackingId;
    private OrderShippedStatus() : this(Guid.Empty, null) { }
}

public class OrderDeliveryRecievedStatus : OrderStatusChange
{
    public DeliveryMember DeliveryMember { get; private set; }

    public OrderDeliveryRecievedStatus(Guid orderId, DeliveryMember deliveryMember) : base(orderId, OrderState.DeliveryRecieved)
    {
        DeliveryMember = deliveryMember;
    }

    public override object AdditionalInfo => DeliveryMember;
    private OrderDeliveryRecievedStatus() : this(Guid.Empty, null) { }
}

public class OrderDeliveredStatus(Guid orderId) : OrderStatusChange(orderId, OrderState.CustomerDelivered)
{

    private OrderDeliveredStatus() : this(Guid.Empty) { }
}