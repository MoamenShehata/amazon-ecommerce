using Amazon.SharedKernel.Customers;
using Amazon.SharedKernel.Orders.Commands;
using Amazon.Shipping.Domain.Companies;
using Amazon.Shipping.Domain.Events;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Shipping.Domain;

public class ShipmentRequest : AuditableAggregate<Guid>, IEntity<Guid>
{
    public Guid OrderId { get; private set; }
    public CustomerInfo Customer { get; private set; }
    public DeliveryAddress ToAddress { get; set; }

    public Guid? AssignedToCompanyId { get; private set; }
    public void AssignedToCompany(ShippingCompany company)
    {
        AssignedToCompanyId = company.Id;
        RaiseEvent(new ShipmentAssignedToCompanyEvent(OrderId, Id, company.Id));
    }

    public ShipmentRequest(Guid orderId, CustomerInfo customer, DeliveryAddress toAddress) : base(Guid.NewGuid())
    {
        OrderId = orderId;
        Customer = customer;
        ToAddress = toAddress;
        AssignedToCompanyId = null;

        RaiseEvent(new ShipmentCreatedEvent(orderId, Id));
    }

    private ShipmentRequest() : base(Guid.Empty)
    {

    }
}