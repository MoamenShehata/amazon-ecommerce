using Amazon.Customers.Domain.ValueObjects;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Customers.Domain;

public class Customer : AuditableAggregate<Guid>, IEntity<Guid>
{
    public ContactInfo ContactInfo { get; private set; }
    public ShippingInfo ShippingInfo { get; private set; }
    public Customer(Guid id, ContactInfo contactInfo) : base(id)
    {
        ContactInfo = contactInfo;
        ShippingInfo = new();
    }

    private Customer() : base(Guid.Empty)
    {

    }
}
