using Amazon.SharedKernel.Customers;
using Amazon.Shipping.Domain.Events;
using Amazon.Shipping.Domain.ValueObjects;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Shipping.Domain;

public class ShipmentRequest : AuditableAggregate<Guid>, IEntity<Guid>
{
    public Guid OrderId { get; private set; }
    public CustomerInfo Customer { get; private set; }
    public ValueObjects.DeliveryAddress ToAddress { get; set; }

    public ShipmentRequest(Guid orderId, CustomerInfo customer, ValueObjects.DeliveryAddress toAddress) : base(Guid.NewGuid())
    {
        OrderId = orderId;
        Customer = customer;
        ToAddress = toAddress;

        RaiseEvent(new ShipmentRequestCreatedEvent(Id));
    }

    private ShipmentRequest() : base(Guid.Empty)
    {

    }
}