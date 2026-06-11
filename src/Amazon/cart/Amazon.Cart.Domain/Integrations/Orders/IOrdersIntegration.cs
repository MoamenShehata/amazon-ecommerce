using Amazon.Cart.Domain.Integrations.Orders.Dtos;
using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.SharedKernel.API;

namespace Amazon.Cart.Domain.Integrations.Orders;

public interface IOrdersIntegration
{
    Task<RestResponse<OrderDto>> CreateAsync(ShoppingCart cart);
}