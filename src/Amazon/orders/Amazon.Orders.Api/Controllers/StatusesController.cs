using Amazon.Orders.Application.Orders;
using Amazon.Orders.Application.Orders.Validators;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Orders.ValueObjects.Status;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Amazon.Orders.Api.Controllers;

[Authorize]
[Route("api/orders/{orderId}/[controller]")]
public class StatusesController(OrdersAppService _ordersAppService) : ApiControllerBase
{
    [HttpPut]
    public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] UpdateOrderStatusRequest request)
    {
        var jsonElement = (JsonElement)request.Payload;

        if (request.To == OrderState.ShippingStarted)
        {
            var payload = jsonElement.Deserialize<ShippingCompanyInfo>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var validationResult = new ShippingCompanyInfoValidator().Validate(payload);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.FirstOrDefault().ErrorMessage);
            request.Payload = payload;
        }

        if (request.To == OrderState.Shipped)
        {
            var payload = request.Payload.ToString();
            if (string.IsNullOrWhiteSpace(payload))
                return BadRequest($"Invalid tracking id");
        }

        if (request.To == OrderState.DeliveryRecieved)
        {
            var payload = jsonElement.Deserialize<DeliveryMember>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var validationResult = new DeliveryMemberValidator().Validate(payload);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            request.Payload = payload;
        }

        return RestResult(await _ordersAppService.UpdateStatusAsync(UserId, orderId, request));
    }
}
