using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;
using Amazon.Customers.Infrastructure.Integrations.Lookups.Dtos;

namespace Amazon.Customers.Infrastructure.Integrations.Lookups;

public interface ILookupsIntegrationClient
{
    Task<List<Country>> GetCountriesAsync();
    Task<HttpContent> GetCountryAsync(int countryId);
}

public class LookupsRestClient(
    HttpClient _httpClient,
    IMemoryCache _memoryCache) : ILookupsIntegrationClient
{
    public async Task<List<Country>> GetCountriesAsync()
    {
        return await _memoryCache.GetOrCreateAsync("countries", async (x) =>
        {
            var json = await _httpClient.GetAsync("countries");

            return await json.Content.ReadFromJsonAsync<List<Country>>();
        });
    }

    public async Task<HttpContent> GetCountryAsync(int countryId) => (await _httpClient.GetAsync($"countries/{countryId}")).Content;
}