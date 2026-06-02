using Amazon.Orders.Application.Orders;
using Amazon.Orders.Application.Orders.Dtos;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Orders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(OrdersAppService _service) : ApiControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCustomerOrdersPage([FromQuery] SearchOrdersRequest pageRequest)
    {
        return Ok(await _service.GetCustomerOrdersPageAsync(UserId, pageRequest));
    }

    [HttpGet("{id}", Name = "GetOrderById")]
    public async Task<IActionResult> GetOrderDetails(Guid id) => RestResult(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromBody] OrderCreateDto request)
    {
        var result = await _service.PlaceAsync(UserId, UserEmail, request);
        if (result.IsSuccess)
            return CreatedAtRoute("GetOrderById", new { id = result.Value.Id }, result.Value);

        return RestResult(result);
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelOrder(Guid id) => RestResult(await _service.CancelAsync(id));

    [HttpPut("{id}/process")]
    public async Task<IActionResult> StartProcessingOrder(Guid id) => RestResult(await _service.StartProcessingAsync(id));

    [HttpPut("{id}/startShipping")]
    public async Task<IActionResult> StartShippingOrder(Guid id) => RestResult(await _service.StartShippingAsync(id));

    [HttpPut("{id}/shipped")]
    public async Task<IActionResult> ShippingCompleted(Guid id) => RestResult(await _service.ShippingCompletedAsync(id));

    [HttpPut("{id}/deliveryAccepted")]
    public async Task<IActionResult> DeliveryAccepted(Guid id) => RestResult(await _service.DeliveryAcceptedAsync(id));

    [HttpPut("{id}/completed")]
    public async Task<IActionResult> CustomerDelivered(Guid id) => RestResult(await _service.CompletedAsync(id));
}
