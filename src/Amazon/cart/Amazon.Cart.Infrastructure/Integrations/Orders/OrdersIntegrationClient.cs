using Amazon.Cart.Domain.Integrations.Orders.Dtos;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Amazon.Cart.Infrastructure.Integrations.Orders;

public class OrdersIntegrationClient(HttpClient _httpClient)
{
    public async Task<HttpContent> CreateAsync(object orderCreateRequest)
    {
        var requestAsJson = JsonSerializer.Serialize(orderCreateRequest);

        var response = await _httpClient.PostAsync($"orders", new StringContent(requestAsJson, new MediaTypeHeaderValue("application/json")));

        return response.Content;
    }
}