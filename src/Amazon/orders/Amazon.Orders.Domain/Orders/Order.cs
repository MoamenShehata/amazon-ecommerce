using Amazon.Orders.Domain.Orders.ValueObjects;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Orders.Domain.Orders;

public class Order : AuditableAggregate<Guid>, IEntity<Guid>
{
    public CustomerInfo Customer { get; private set; }

    private List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> Items => [.. _orderItems];

    public Order(CustomerInfo customer, List<OrderItem> orderItems) : base(Guid.NewGuid())
    {
        Customer = customer;
        _orderItems = orderItems;
    }

    public decimal Price => _orderItems.Sum(x => x.Price);
    public int UniqueItemsCount => _orderItems.Count;

    #region Infra
    private Order() : base(Guid.Empty) { }
    #endregion
}