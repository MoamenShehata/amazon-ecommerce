using Amazon.Customers.Domain;
using Amazon.Customers.Domain.ValueObjects;
using Amazon.SharedKernel.API;

namespace Amazon.Customers.Infrastructure.Integrations;

internal class Country
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<City> Cities { get; set; }
}

internal class City
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class AddressService : IAddressService
{
    private readonly List<Country> _countries;
    public AddressService()
    {
        _countries = new() {
            new Country
            {
                Id = 1,
                Name = "Egypt",
                Cities = new List<City>
                {
                    new City { Id = 1, Name = "Sharqia"},
                    new City { Id = 2, Name = "10th Of Ramdan"},
                    new City { Id = 3, Name = "Cairo"},
                }
            },
            new Country
            {
                Id = 2,
                Name = "Lebanon",
                Cities = new List<City>
                {
                    new City { Id = 1, Name = "City1"},
                    new City { Id = 2, Name = "City2"},
                    new City { Id = 3, Name = "City3"},
                }
            }
        };
    }

    public async Task<RestResponse<CityLookup>> GetCityInfoAsync(int countryId, int cityId)
    {
        // ACL, but for now let`s get data from memory

        var country = _countries.SingleOrDefault(x => x.Id == countryId);
        if (country is null)
            return RestResponse<CityLookup>.BadRequest($"Country with id {countryId} not found");

        var city = country.Cities.SingleOrDefault(x => x.Id == cityId);
        if (country is null)
            return RestResponse<CityLookup>.BadRequest($"Country with id {country} does not have a city with id {cityId}");

        return RestResponse<CityLookup>.Success(new CityLookup(country.Name, city.Name, city.Id));
    }

    public async Task<RestResponse<List<CountryLookup>>> GetCountriesInfoAsync(int[] countryIds)
    {
        var countries = _countries.Where(x => countryIds.Contains(x.Id)).ToList();
        if (countries.Count != countryIds.Length)
            return RestResponse<List<CountryLookup>>.BadRequest($"Some Countries were not found");

        return RestResponse<List<CountryLookup>>.Success(countries.Select(c => new CountryLookup(c.Id, c.Name, c.Cities.Select(ct => new CityLookup(c.Name, ct.Name, ct.Id)).ToList())).ToList());
    }
}
