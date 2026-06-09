using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Shipping.Domain.Companies;

public class ShippingCompany : AuditableAggregate<Guid>, IEntity<Guid>
{
    public string? Website { get; private set; }
    public string PhoneNumber { get; private set; }
    public string ContactEmail { get; private set; }
    public string RepresentativeEmail { get; private set; }
    public bool IsActive { get; private set; }

    public ShippingCompany(string? website, string phoneNumber, string contactEmail, bool isActive, string representativeEmail) : base(Guid.NewGuid())
    {
        Website = website;
        PhoneNumber = phoneNumber;
        ContactEmail = contactEmail;
        IsActive = isActive;
        RepresentativeEmail = representativeEmail;
    }

    public void Deactivate() => IsActive = false;

    private ShippingCompany() : base(Guid.Empty) { }
}