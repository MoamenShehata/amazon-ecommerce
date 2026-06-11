using Amazon.Cart.Domain.Integrations.Customers;
using Amazon.Cart.Domain.Integrations.Orders;
using Amazon.Cart.Domain.Integrations.Orders.Dtos;
using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.SharedKernel.API;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Amazon.Cart.Infrastructure.Integrations.Orders;

public class OrderIntegration(
    OrdersIntegrationClient _ordersClient,
    ICustomersIntegration _customerIntegration,
    ILogger<OrderIntegration> _logger
    ) : IOrdersIntegration
{
    public async Task<RestResponse<OrderDto>> CreateAsync(ShoppingCart cart)
    {
        var orderId = Guid.NewGuid();

        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["correlationId"] = orderId,
        });

        try
        {
            var deliveryAddressResult = await _customerIntegration.GetDeliveryAddressOrDefaultAsync(cart.DeliverToAddressId);

            var jsonResponse = await _ordersClient.CreateAsync(new { OrderId = orderId, DeliveryAddress = deliveryAddressResult.Value, ShoppingCart = cart.Items.Select(x => new KeyValuePair<Guid, int>(x.ProductId, x.Quantity)).ToList() });

            return RestResponse<OrderDto>.Success(await jsonResponse.ReadFromJsonAsync<OrderDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error happened while creating order for cart {cartId}", cart.Id);
            return RestResponse<OrderDto>.Failure($"Error happened while creating order for cart {cart.Id}");
        }

    }
}