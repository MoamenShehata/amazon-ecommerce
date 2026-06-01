using Amazon.Cart.Domain.Services;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Amazon.Cart.Infrastructure.Integrations;

internal class OrderCreatedDto
{
    public Guid Id { get; set; }
}
public class OrderService(IHttpClientFactory _httpClientFactory) : IOrderService
{
    public async Task<Guid> CreateOrderAsync(Guid userId, string email, List<KeyValuePair<Guid, int>> shoppingCart, object PaymentInfo, object DeliveryAddress)
    {
        using var client = _httpClientFactory.CreateClient();

        var requestBody = new
        {
            UserId = userId,
            Email = email,
            ShoppingCart = shoppingCart,
            PaymentInfo,
            DeliveryAddress
        };

        var requestAsJson = JsonSerializer.Serialize(requestBody);

        var response = await client.PostAsync($"https://localhost:7270/api/orders", new StringContent(requestAsJson, new MediaTypeHeaderValue("application/json")));
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadFromJsonAsync<OrderCreatedDto>();
        return responseBody.Id;
    }
}