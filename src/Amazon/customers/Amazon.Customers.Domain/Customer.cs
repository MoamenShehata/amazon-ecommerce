using Amazon.Customers.Domain.Entities;
using Amazon.Customers.Domain.Events;
using Amazon.Customers.Domain.ValueObjects;
using Amazon.SharedKernel.API;
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

    public RestResponse<bool> AddShippingAddress(ShippingAddress newAddress)
    {
        var addAddressResult = ShippingInfo.AddAddress(newAddress);
        if (!addAddressResult.IsSuccess)
            return RestResponse<bool>.BadRequest(addAddressResult.Error.ToString());

        RaiseEvent(new CustomerShippingInfoChangedEvent(Id));
        return RestResponse<bool>.Success(true);
    }

    public RestResponse<bool> RemoveShippingAddress(int addressId)
    {
        var removeAddressResult = ShippingInfo.RemoveAddress(addressId);
        if (!removeAddressResult.IsSuccess)
            return RestResponse<bool>.BadRequest(removeAddressResult.Error.ToString());

        RaiseEvent(new CustomerShippingInfoChangedEvent(Id));
        return RestResponse<bool>.Success(true);
    }

    #region Infrastructure

    private Customer() : base(Guid.Empty)
    {

    }
    #endregion
}
