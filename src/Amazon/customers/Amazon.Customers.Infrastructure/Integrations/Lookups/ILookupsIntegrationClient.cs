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
    private const string basePath = "lookups/countries";

    public async Task<List<Country>> GetCountriesAsync()
    {
        return await _memoryCache.GetOrCreateAsync("countries", async (x) =>
        {
            var json = await _httpClient.GetAsync($"{basePath}?pageNumber=1&pageSize=300");

            var page = await json.Content.ReadFromJsonAsync<GetCountriesPageDto>();

            return page.Items;
        });
    }

    public async Task<HttpContent> GetCountryAsync(int countryId) => (await _httpClient.GetAsync($"{basePath}/{countryId}")).Content;
}