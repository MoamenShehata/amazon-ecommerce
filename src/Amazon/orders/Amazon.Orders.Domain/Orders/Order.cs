using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Orders.ValueObjects.Status;
using Amazon.SharedKernel.Orders.Events;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Orders.Domain.Orders;

public class Order : AuditableAggregate<Guid>, IEntity<Guid>
{
    public CustomerInfo Customer { get; private set; }

    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> Items => _orderItems.AsReadOnly();

    public Order(Guid orderId, CustomerInfo customer, List<OrderItem> orderItems) : base(orderId)
    {
        Customer = customer;
        _orderItems = orderItems;
        UpdateStatus(new OrderCreatedStatus(orderId));
    }

    public decimal Price => _orderItems.Sum(x => x.Price);
    public int UniqueItemsCount => _orderItems.Count;


    private readonly ICollection<OrderStatusChange> _history = new HashSet<OrderStatusChange>();
    internal void UpdateStatus(OrderStatusChange newStatus) => _history.Add(newStatus);
    public OrderStatusChange Status => _history.OrderByDescending(x => x.CreatedAt).FirstOrDefault();

    public void Cancel()
    {
        UpdateStatus(new OrderCancelledStatus(Id));

        RaiseEvent(new OrderCancelledEvent(Id));
    }

    public void StartShipping(ShippingCompanyInfo shippingCompanyInfo)
    {
        UpdateStatus(new OrderShippingStartedStatus(Id, shippingCompanyInfo));

        RaiseEvent(new OrderShippingStartedEvent(Id));
    }


    #region Infra
    private Order() : base(Guid.Empty) { }
    #endregion
}