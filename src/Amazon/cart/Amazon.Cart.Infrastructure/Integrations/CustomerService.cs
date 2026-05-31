using Amazon.Cart.Domain.Integrations;
using Amazon.SharedKernel.API;
using System.Net.Http.Json;

namespace Amazon.Cart.Infrastructure.Integrations;

internal class GetCustomerProfileResponse
{
    public Guid CustomerId { get; set; }
    public List<CustomerDeliveryAddress> Addresses { get; set; }
}

public class CustomerService(IHttpClientFactory _httpClientFactory) : ICustomerService
{
    public async Task<RestResponse<CustomerDeliveryAddress>> GetCustomerDeliveryAddressOrDefaultAsync(Guid userId, int? addressId)
    {
        using var client = _httpClientFactory.CreateClient();

        var response = await client.GetAsync($"https://localhost:7128/api/customers/{userId}");
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadFromJsonAsync<GetCustomerProfileResponse>();
        var address = responseBody.Addresses.FirstOrDefault(a => (addressId.HasValue && a.Id == addressId) || true);

        return RestResponse<CustomerDeliveryAddress>.Success(address);
    }
}
