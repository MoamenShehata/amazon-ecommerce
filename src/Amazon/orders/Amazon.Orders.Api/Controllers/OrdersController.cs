using Amazon.Orders.Application.Orders;
using Amazon.Orders.Application.Orders.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Orders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(OrdersAppService _service) : ApiControllerBase
{
    //[HttpGet]
    //public async Task<IActionResult> GetCategories(Guid id, [FromQuery] PageRequest pageRequest)
    //{
    //    return Ok(await _categoriesAppService.GetPageAsync(pageRequest));
    //}

    [HttpGet("{id}", Name = "GetOrderById")]
    public async Task<IActionResult> GetCategory(Guid id) => RestResult(await _service.GetByIdAsync(id));

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
