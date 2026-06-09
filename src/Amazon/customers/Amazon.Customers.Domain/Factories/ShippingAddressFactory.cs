using Amazon.Customers.Domain.Entities;
using Amazon.Customers.Domain.ValueObjects;
using Amazon.SharedKernel.Customers;

namespace Amazon.Customers.Domain.Factories;

public class ShippingAddressFactory
{
    public ShippingAddress Create(Guid customerId, CityInfo city, HouseInfo house, bool isDefault)
    {
        // validate cityinfo
        return new ShippingAddress(customerId, city, house, isDefault);
    }
}
