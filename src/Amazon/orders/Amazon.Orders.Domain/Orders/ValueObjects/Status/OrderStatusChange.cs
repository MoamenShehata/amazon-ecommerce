using Amazon.SharedKernel.API;
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

    internal RestResponse<OrderStatusChange> CanUpdateTo(OrderState state, object withPayload)
    {
        if (state == OrderState.Cancelled)
            throw new InvalidOperationException("Order cancellation is a seperate process and should be handled indepednetly");

        var nextCandidateStatusResult = CreateNextStatus(withPayload);

        if (nextCandidateStatusResult.Value is null || nextCandidateStatusResult.Value.State != state)
            return RestResponse<OrderStatusChange>.BadRequest($"Order status cannot be updated to {state}");

        if (!nextCandidateStatusResult.IsSuccess)
            return nextCandidateStatusResult;

        return RestResponse<OrderStatusChange>.Success(nextCandidateStatusResult);
    }

    protected abstract RestResponse<OrderStatusChange> CreateNextStatus(object payload);

    #region Infrastucture
    private OrderStatusChange() { }

    #endregion
}

public class OrderPendingStatus(Guid orderId) : OrderStatusChange(orderId, OrderState.Pending)
{
    private OrderPendingStatus() : this(Guid.Empty) { }
    public override bool CanBeCancelled => true;

    protected override RestResponse<OrderStatusChange> CreateNextStatus(object payload) => RestResponse<OrderStatusChange>.Success(new OrderProcessingStatus(OrderId));
}

public class OrderCancelledStatus(Guid orderId) : OrderStatusChange(orderId, OrderState.Cancelled)
{
    private OrderCancelledStatus() : this(Guid.Empty) { }
    public override bool CanBeCancelled => false;
    protected override RestResponse<OrderStatusChange> CreateNextStatus(object payload) => RestResponse<OrderStatusChange>.BadRequest("Order is canncelled and cannot be handled in anyway!");
}

public class OrderProcessingStatus(Guid orderId) : OrderStatusChange(orderId, OrderState.Processing)
{
    private OrderProcessingStatus() : this(Guid.Empty) { }
    public override bool CanBeCancelled => false;
    protected override RestResponse<OrderStatusChange> CreateNextStatus(object payload)
    {
        if (payload is not ShippingCompanyInfo shippingInfo)
            return RestResponse<OrderStatusChange>.BadRequest("Shipping Info payload is invalid!");

        return RestResponse<OrderStatusChange>.Success(new OrderShippingStartedStatus(OrderId, shippingInfo));
    }
}

public class OrderShippingStartedStatus : OrderStatusChange
{
    public ShippingCompanyInfo CompanyInfo { get; private set; }
    public OrderShippingStartedStatus(Guid orderId, ShippingCompanyInfo companyInfo) : base(orderId, OrderState.ShippingStarted)
    {
        CompanyInfo = companyInfo;
    }

    public override object AdditionalInfo => CompanyInfo;
    public override bool CanBeCancelled => false;

    private OrderShippingStartedStatus() : this(Guid.Empty, null) { }

    protected override RestResponse<OrderStatusChange> CreateNextStatus(object payload)
    {
        //if payload is not valid tracking number string
        //return RestResponse<OrderStatusChange>.BadRequest("Invalid tracking id!");

        return RestResponse<OrderStatusChange>.Success(new OrderShippedStatus(OrderId, payload.ToString()));
    }
}

public class OrderShippedStatus : OrderStatusChange
{
    public string TrackingId { get; private set; }
    public OrderShippedStatus(Guid orderId, string trackingId) : base(orderId, OrderState.Shipped)
    {
        TrackingId = trackingId;
    }

    public override object AdditionalInfo => new { TrackingId };

    public override bool CanBeCancelled => false;
    private OrderShippedStatus() : this(Guid.Empty, null) { }

    protected override RestResponse<OrderStatusChange> CreateNextStatus(object payload)
    {
        if (payload is not DeliveryMember deliveryMemberInfo)
            return RestResponse<OrderStatusChange>.BadRequest("Delivery Member payload is invalid!");

        return RestResponse<OrderStatusChange>.Success(new OrderDeliveryRecievedStatus(OrderId, deliveryMemberInfo));
    }
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

    protected override RestResponse<OrderStatusChange> CreateNextStatus(object payload)
    {
        return RestResponse<OrderStatusChange>.Success(new OrderDeliveredStatus(OrderId));
    }
}

public class OrderDeliveredStatus(Guid orderId) : OrderStatusChange(orderId, OrderState.CustomerDelivered)
{
    public override bool CanBeCancelled => false;

    private OrderDeliveredStatus() : this(Guid.Empty) { }
    protected override RestResponse<OrderStatusChange> CreateNextStatus(object payload) => RestResponse<OrderStatusChange>.BadRequest("Order is completed and cannot be updated to any status!");
}