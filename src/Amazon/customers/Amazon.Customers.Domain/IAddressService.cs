using Amazon.Customers.Domain.ValueObjects;
using Amazon.SharedKernel.API;

namespace Amazon.Customers.Domain;

public interface IAddressService
{
    Task<RestResponse<CityLookup>> GetCityInfoAsync(int countryId, int cityId);
    Task<RestResponse<List<CountryLookup>>> GetCountriesInfoAsync(int[] countryIds);
}