using Amazon.Orders.Domain.Orders.ValueObjects;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Orders.Domain.Orders;

public class Order : AuditableAggregate<Guid>, IEntity<Guid>
{
    private List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> Items => [.. _orderItems];

    public Order(List<OrderItem> orderItems) : base(Guid.NewGuid())
    {
        _orderItems = orderItems;
    }


    private Order() : base(Guid.Empty) { }
}