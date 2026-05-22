using Amazon.Customers.Application.CustomerProfiles.Models;
using Amazon.Customers.Domain;
using Amazon.Customers.Domain.ValueObjects;
using Amazon.SharedKernel.API;

namespace Amazon.Customers.Application;

public class ShippingAddresAdapter(IAddressService _addressService)
{
    public async Task<RestResponse<ICollection<CustomerProfileAddress>>> ToReadModel(ShippingInfo customerShippingInfo)
    {
        var countriesResult = await _addressService.GetCountriesInfoAsync(customerShippingInfo.ShippingAddresses.Select(a => a.City.CountryId).Distinct().ToArray());
        if (!countriesResult.IsSuccess)
            return RestResponse<ICollection<CustomerProfileAddress>>.NotFound(countriesResult.Error);

        var result = new List<CustomerProfileAddress>();

        foreach (var address in customerShippingInfo.ShippingAddresses)
        {
            var country = countriesResult.Value.FirstOrDefault(c => c.Id == address.City.CountryId);
            var city = country.Cities.FirstOrDefault(c => c.CityId == address.City.CityId);

            result.Add(new CustomerProfileAddress(country.Name, city.CityName, address.House.Street, address.House.BuildingNumber, address.House.ApartmentNumber));
        }

        return RestResponse<ICollection<CustomerProfileAddress>>.Success(result);
    }
}