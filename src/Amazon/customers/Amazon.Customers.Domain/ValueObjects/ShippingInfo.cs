using Amazon.Customers.Domain.Entities;
using Amazon.SharedKernel.API;

namespace Amazon.Customers.Domain.ValueObjects;

public class ShippingInfo
{
    private const int MaxShippingAddresses = 5;
    private const int MinShippingAddresses = 1;

    private readonly List<ShippingAddress> _shippingAddresses = new();

    internal RestResponse<bool> AddAddress(ShippingAddress newAddress)
    {
        if (ShippingAddresses.Count == MaxShippingAddresses)
            return RestResponse<bool>.BadRequest($"A customer cannot have more than {MaxShippingAddresses} shipping addresses.");

        _shippingAddresses.Add(newAddress);
        EnsureOneDefaultAddress();

        return RestResponse<bool>.Success(true);
    }

    internal RestResponse<bool> RemoveAddress(int addressId)
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

        var candidateAddress = DefaultAddress ?? FirstAddress;
        candidateAddress.SetAsDefault();

        foreach (var address in _shippingAddresses.Where(a => a != candidateAddress))
            address.UnsetAsDefault();
    }

    public IReadOnlyCollection<ShippingAddress> ShippingAddresses => _shippingAddresses.OrderByDescending(x => x.CreatedOn).ToList();
    internal ShippingAddress DefaultAddress => ShippingAddresses.FirstOrDefault(a => a.IsDefault);
    internal ShippingAddress FirstAddress => ShippingAddresses.LastOrDefault();
}