using Amazon.Cart.Application;
using Amazon.Cart.Application.Dtos;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Cart.Api.Controllers;

public class CartsController(CartService _cartService) : ApiControllerBase
{
    [HttpGet("{cartId}")]
    public async Task<IActionResult> CreateShoppingCart(Guid cartId)
    {
        var result = await _cartService.GetByIdAsync(cartId);
        if (result.IsSuccess)
            return Ok(result.Value);

        return RestResult(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateShoppingCart([FromBody] CartCreateDto cartModel)
    {
        var result = await _cartService.CreateCartAsync(cartModel);
        if (result.IsSuccess)
            return Ok(result.Value);

        return RestResult(result);
    }
}
