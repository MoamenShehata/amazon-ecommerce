using Amazon.Cart.Domain.Integrations.Orders.Dtos;

namespace Amazon.Cart.Domain.Integrations.Orders;

public interface IOrdersIntegration
{
    Task<OrderDto> CreateAsync(OrderCreateDto orderDto);
}