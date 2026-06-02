using Amazon.Cart.Domain.Integrations.Orders;
using Amazon.Cart.Domain.Integrations.Orders.Dtos;
using System.Net.Http.Json;

namespace Amazon.Cart.Infrastructure.Integrations.Orders;

public class OrderIntegration(OrdersIntegrationClient _ordersClient) : IOrdersIntegration
{
    public async Task<OrderDto> CreateAsync(OrderCreateDto orderDto)
    {
        var jsonResponse = await _ordersClient.CreateAsync(orderDto);

        return await jsonResponse.ReadFromJsonAsync<OrderDto>();
    }
}