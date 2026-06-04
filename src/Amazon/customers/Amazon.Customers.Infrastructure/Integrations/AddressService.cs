using Amazon.Customers.Domain;
using Amazon.Customers.Domain.ValueObjects;
using Amazon.Customers.Infrastructure.Integrations.Lookups;
using Amazon.Customers.Infrastructure.Integrations.Lookups.Dtos;
using Amazon.SharedKernel.API;
using System.Net.Http.Json;

namespace Amazon.Customers.Infrastructure.Integrations;

public class AddressService(ILookupsIntegrationClient _lookupsClient) : IAddressService
{
    public async Task<RestResponse<List<CountryLookup>>> GetCountriesInfoAsync(int[] countryIds)
    {
        var countries = (await _lookupsClient.GetCountriesAsync()).Where(x => countryIds.Contains(x.Id)).ToList();
        if (countries.Count != countryIds.Length)
            return RestResponse<List<CountryLookup>>.BadRequest($"Some Countries were not found");

        return RestResponse<List<CountryLookup>>.Success(countries.Select(c => new CountryLookup(c.Id, c.Name, c.Cities.Select(ct => new CityLookup(c.Name, ct.Name, ct.Id)).ToList())).ToList());
    }

    public async Task<RestResponse<CityLookup>> GetCityInfoAsync(int countryId, int cityId)
    {
        var country = await (await _lookupsClient.GetCountryAsync(countryId)).ReadFromJsonAsync<Country>();
        if (country is null)
            return RestResponse<CityLookup>.BadRequest($"Country with id {countryId} not found");

        var city = country.Cities.SingleOrDefault(x => x.Id == cityId);
        if (country is null)
            return RestResponse<CityLookup>.BadRequest($"Country with id {country} does not have a city with id {cityId}");

        return RestResponse<CityLookup>.Success(new CityLookup(country.Name, city.Name, city.Id));
    }
}
