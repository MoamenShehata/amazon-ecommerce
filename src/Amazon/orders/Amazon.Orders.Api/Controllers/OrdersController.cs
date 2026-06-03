using Amazon.Orders.Application.Orders;
using Amazon.Orders.Application.Orders.Dtos;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Orders.Api.Controllers;

[Authorize]
public class OrdersController(OrdersAppService _service) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCustomerOrdersPage([FromQuery] SearchOrdersRequest pageRequest)
    {
        return Ok(await _service.GetCustomerOrdersPageAsync(UserId, pageRequest));
    }

    [HttpGet("{id}", Name = "GetOrderById")]
    public async Task<IActionResult> GetOrderDetails(Guid id) => RestResult(await _service.GetByUserAsync(UserId, id));

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromBody] OrderCreateDto request)
    {
        var result = await _service.PlaceAsync(UserId, UserEmail, request);
        if (result.IsSuccess)
            return CreatedAtRoute("GetOrderById", new { id = result.Value.Id }, result.Value);

        return RestResult(result);
    }


    [HttpDelete("{orderId}")]
    public async Task<IActionResult> CancelOrder(Guid orderId) => RestResult(await _service.CancelAsync(UserId, orderId));
}
