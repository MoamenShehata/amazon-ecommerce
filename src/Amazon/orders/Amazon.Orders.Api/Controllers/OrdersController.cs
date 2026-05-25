using Amazon.Orders.Application.Orders;
using Amazon.Orders.Application.Orders.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Orders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(OrdersAppService _service) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCustomerOrdersPage([FromQuery] SearchOrdersRequest pageRequest)
    {
        var userId = Guid.Parse("5b32881f-dac9-4f88-ac0c-6e770afc85ce"); // should come from jwt
        return Ok(await _service.GetCustomerOrdersPageAsync(userId, pageRequest));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderDetails(Guid id) => RestResult(await _service.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromBody] OrderCreateDto request)
    {
        var result = await _service.PlaceAsync(request);
        if (result.IsSuccess)
            return CreatedAtRoute("GetOrderById", new { id = result.Value.Id }, result.Value);

        return RestResult(result);
    }

    //[HttpPut("{id}")]
    //public async Task<IActionResult> UpdateCategory(
    //    Guid id,
    //    [FromBody] UpdateCategoryRequest request)
    //{
    //    return RestResult(await _categoriesAppService.UpdateAsync(id, request));
    //}

    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeleteCategory(
    //    Guid id,
    //    [FromQuery] Guid? orphanProductsNewCategoryId = null)
    //{
    //    return RestResult(await _categoriesAppService.DeleteAsync(id, orphanProductsNewCategoryId));
    //}
}
