using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Orders.ValueObjects.Status;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Customers;
using Amazon.SharedKernel.Extensions;
using Amazon.SharedKernel.Orders.Events;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Orders.Domain.Orders;

public class Order : AuditableAggregate<Guid>, IEntity<Guid>
{
    public CustomerInfo Owner { get; private set; }
    public string PaymentInfo { get; private set; }
    public DeliveryAddress DeliveryAddress { get; private set; }


    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> Items => _orderItems.AsReadOnly();

    public Order(Guid orderId, CustomerInfo customer, List<OrderItem> orderItems, string paymentInfo, DeliveryAddress deliveryAddress) : base(orderId)
    {
        Owner = customer;
        _orderItems = orderItems;
        PaymentInfo = paymentInfo;
        DeliveryAddress = deliveryAddress;
        UpdateStatus(new OrderCreatedStatus(orderId));
    }

    public decimal Price => _orderItems.Sum(x => x.Price);
    public int UniqueItemsCount => _orderItems.Count;


    private readonly ICollection<OrderStatusChange> _history = new HashSet<OrderStatusChange>();
    internal void UpdateStatus(OrderStatusChange newStatus)
    {
        _history.Add(newStatus);

        if (newStatus.State == OrderState.CustomerDelivered)
            RaiseEvent(new OrderCompletedEvent(Id));
    }

    public OrderStatusChange Status => _history.OrderByDescending(x => x.CreatedAt).FirstOrDefault();

    public RestResponse<bool> TryUpdateTo(OrderState state, object withPayload)
    {
        var canUpdateStatusResult = Status.CanUpdateTo(state, withPayload);
        if (!canUpdateStatusResult.IsSuccess)
            return canUpdateStatusResult.MapTo(false);

        UpdateStatus(canUpdateStatusResult.Value);
        return RestResponse<bool>.Success(true);
    }

    public void Cancel()
    {
        UpdateStatus(new OrderCancelledStatus(Id));

        RaiseEvent(new OrderCancelledEvent(Id));
    }

    //public void StartShipping(string trackingId, ShippingCompanyInfo shippingCompanyInfo)
    //{
    //    UpdateStatus(new OrderShippingStartedStatus(Id, trackingId, shippingCompanyInfo));

    //    RaiseEvent(new OrderShippingStartedEvent(Id));
    //}

    //public void DeliveryAccepted(DeliveryMember deliveryMember)
    //{
    //    UpdateStatus(new OrderDeliveryRecievedStatus(Id, deliveryMember));

    //    RaiseEvent(new OrderRecievedByDeliveryGuyEvent(Id, deliveryMember.Name, deliveryMember.PhoneNumber));
    //    // to send sms to the customer
    //}


    #region Infra
    private Order() : base(Guid.Empty) { }
    #endregion
}