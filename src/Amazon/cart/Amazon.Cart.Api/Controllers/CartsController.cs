using Amazon.Cart.Api.Dtos;
using Amazon.Cart.Application;
using Amazon.Cart.Application.Dtos;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Cart.Api.Controllers;

public class CartsController(CartAppService _cartService) : ApiControllerBase
{
    [HttpGet("{cartId}")]
    public async Task<IActionResult> GetShoppingCart(Guid cartId)
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
        return Ok(result);
    }

    [HttpPost("{cartId}/checkoutOtp")]
    public async Task<IActionResult> CheckoutCartUsingOtp(Guid cartId, [FromBody] CheckoutUsingOtpDto request)
    {
        var userId = Guid.Parse("5b32881f-dac9-4f88-ac0c-6e770afc85ce"); // should come from jwt
        var result = await _cartService.CheckoutCartUsingOtpAsync(cartId, request.Otp, userId);
        if (result.IsSuccess)
            return Ok(result.Value);

        return RestResult(result);
    }
}
