using Amazon.Customers.Domain.Entities;
using Amazon.SharedKernel.API;
using Moamen.SDKs.SharedKernel.DDD.Definitions;

namespace Amazon.Customers.Domain;

public class Customer : AuditableAggregate<Guid>
{
    private readonly List<ShippingAddress> _shippingAddresses = new();
    public IReadOnlyCollection<ShippingAddress> ShippingAddresses => _shippingAddresses.OrderByDescending(x => x.CreatedOn).AsReadOnly();

    private const int MaxShippingAddresses = 5;
    private const int MinShippingAddresses = 1;

    public RestResponse<bool> AddAddress(ShippingAddress newAddress)
    {
        if (ShippingAddresses.Count == MaxShippingAddresses)
            return RestResponse<bool>.BadRequest($"A customer cannot have more than {MaxShippingAddresses} shipping addresses.");

        _shippingAddresses.Add(newAddress);
        EnsureOneDefaultAddress();

        return RestResponse<bool>.Success(true);
    }

    public RestResponse<bool> RemoveAddress(int addressId)
    {
        var addressToRemove = _shippingAddresses.FirstOrDefault(x => x.Id == addressId);
        if (addressToRemove == null)
            return RestResponse<bool>.NotFound($"No shipping address found with ID {addressId}.");

        if (_shippingAddresses.Count - 1 < MinShippingAddresses)
            return RestResponse<bool>.BadRequest($"A customer must have at least {MinShippingAddresses} shipping address.");

        _shippingAddresses.Remove(addressToRemove);
        EnsureOneDefaultAddress();
        return RestResponse<bool>.Success(true);
    }

    private void EnsureOneDefaultAddress()
    {
        if (ShippingAddresses.Count == 0) return;

        var candidateAddress = ShippingAddresses.FirstOrDefault(x => x.IsDefault) ?? FirstAddress;

        foreach (var address in _shippingAddresses.Where(a => a != candidateAddress))
            address.UnsetAsDefault();

        candidateAddress.SetAsDefault();
    }

    public ShippingAddress DefaultAddress => ShippingAddresses.FirstOrDefault(addr => addr.IsDefault);
    public ShippingAddress FirstAddress => ShippingAddresses.FirstOrDefault();

    public Customer(Guid id) : base(id)
    {
    }
}
