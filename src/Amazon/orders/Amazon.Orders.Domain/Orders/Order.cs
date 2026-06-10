using Amazon.Orders.Domain.Orders.Entites;
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
    public DeliveryAddress DeliveryAddress { get; private set; }


    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> Items => _orderItems.AsReadOnly();

    public Order(Guid orderId, CustomerInfo customer, List<OrderItem> orderItems, DeliveryAddress deliveryAddress) : base(orderId)
    {
        Owner = customer;
        _orderItems = orderItems;
        DeliveryAddress = deliveryAddress;
        UpdateStatus(new OrderPendingStatus(orderId));
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

    private readonly ICollection<Transaction> _transactions = new HashSet<Transaction>();

    internal Transaction? Transaction => _transactions.SingleOrDefault(x => !x.IsArchived);

    public CheckoutPaymentInfo PaymentInfo => Transaction?.PaymentInfo ?? new CheckoutPaymentInfo();

    public RestResponse<bool> ConfirmPayment(DateTime happenedAt, CheckoutPaymentInfo paymentInfo)
    {
        if (Transaction != null)
            return RestResponse<bool>.BadRequest("Can not cconfirm this order!");

        _transactions.Add(new Transaction(Id, Price, happenedAt, paymentInfo));
        return RestResponse<bool>.Success(true);
    }

    public RestResponse<bool> RequestCompensation()
    {
        if (Transaction is null)
            return RestResponse<bool>.BadRequest("Order is in pending state");

        RaiseEvent(new OrderRefundRequestedEvent(Id));
        Abandon("Could not reserve inventory items");

        return RestResponse<bool>.Success(true);
    }

    private void Abandon(string reason)
    {
        UpdateStatus(new OrderAbandonedStatus(Id, reason));

        RaiseEvent(new OrderAbandonedEvent(Id, reason));
    }





    #region Infra
    private Order() : base(Guid.Empty) { }
    #endregion
}