using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Shipping.Domain.Companies;

public class ShippingCompany : AuditableAggregate<Guid>, IEntity<Guid>
{
    public ShippingCompany(Guid id) : base(id)
    {
    }
}