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

    public abstract bool CanBeCancelled { get; }

    #region Infrastucture
    private OrderStatusChange() { }

    #endregion
}

public class OrderCreatedStatus(Guid orderId) : OrderStatusChange(orderId, OrderState.Created)
{
    private OrderCreatedStatus() : this(Guid.Empty) { }
    public override bool CanBeCancelled => true;
}

public class OrderCancelledStatus(Guid orderId) : OrderStatusChange(orderId, OrderState.Cancelled)
{
    private OrderCancelledStatus() : this(Guid.Empty) { }
    public override bool CanBeCancelled => false;
}

public class OrderProcessingStatus(Guid orderId) : OrderStatusChange(orderId, OrderState.Processing)
{
    private OrderProcessingStatus() : this(Guid.Empty) { }
    public override bool CanBeCancelled => false;
}

public class OrderShippingStartedStatus : OrderStatusChange
{
    public ShippingCompanyInfo CompanyInfo { get; private set; }
    public string TrackingId { get; private set; }
    public OrderShippingStartedStatus(Guid orderId, string trackingId, ShippingCompanyInfo companyInfo) : base(orderId, OrderState.ShippingStarted)
    {
        TrackingId = trackingId;
        CompanyInfo = companyInfo;
    }

    public override object AdditionalInfo => CompanyInfo;
    public override bool CanBeCancelled => false;

    private OrderShippingStartedStatus() : this(Guid.Empty, null, null) { }
}

public class OrderShippedStatus : OrderStatusChange
{
    public OrderShippedStatus(Guid orderId) : base(orderId, OrderState.Shipped)
    {
    }

    public override bool CanBeCancelled => false;
    private OrderShippedStatus() : this(Guid.Empty) { }
}

public class OrderDeliveryRecievedStatus : OrderStatusChange
{
    public DeliveryMember DeliveryMember { get; private set; }

    public OrderDeliveryRecievedStatus(Guid orderId, DeliveryMember deliveryMember) : base(orderId, OrderState.DeliveryRecieved)
    {
        DeliveryMember = deliveryMember;
    }

    public override object AdditionalInfo => DeliveryMember;
    public override bool CanBeCancelled => false;
    private OrderDeliveryRecievedStatus() : this(Guid.Empty, null) { }
}

public class OrderDeliveredStatus(Guid orderId) : OrderStatusChange(orderId, OrderState.CustomerDelivered)
{
    public override bool CanBeCancelled => false;

    private OrderDeliveredStatus() : this(Guid.Empty) { }
}