using Amazon.SharedKernel.Customers;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Customers.Domain.Entities;

public class ShippingAddress : AuditableEntity<int>
{
    public Guid CustomerId { get; private set; }

    public CityInfo City { get; private set; }
    public HouseInfo House { get; private set; }

    public bool IsDefault { get; private set; }
    public void SetAsDefault() => IsDefault = true;
    public void UnsetAsDefault() => IsDefault = false;


    internal ShippingAddress(Guid customerId, CityInfo city, HouseInfo house, bool isDefault) : base(0)
    {
        CustomerId = customerId;
        City = city;
        House = house;
        IsDefault = isDefault;
    }

    private ShippingAddress() : base(0)
    {

    }
}