using Amazon.Cart.Application;
using Amazon.Cart.Application.Dtos;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Cart.Api.Controllers;

public class CartsController(CartAppService _cartService) : ApiControllerBase
{
    [HttpGet("{cartId}")]
    public async Task<IActionResult> GetById(Guid cartId)
    {
        var result = await _cartService.GetByIdAsync(cartId);
        if (result.IsSuccess)
            return Ok(result.Value);

        return RestResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CartCreateDto cartModel)
    {
        var result = await _cartService.CreateCartAsync(cartModel);
        return Ok(result);
    }
}
