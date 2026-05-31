using Amazon.Cart.Api.Dtos;
using Amazon.Cart.Application;
using Amazon.Cart.Application.Dtos;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Policy = "CARTS_POLICY")]
    [HttpPost("{cartId}/checkoutOtp/{paymentRequestId}")]
    public async Task<IActionResult> CheckoutCartUsingOtp(Guid cartId, Guid paymentRequestId, [FromBody] CheckoutUsingOtpDto request)
    {
        var result = await _cartService.CheckoutCartUsingOtpAsync(cartId, paymentRequestId, request.Otp);
        if (result.IsSuccess)
            return Ok(result.Value);

        return RestResult(result);
    }
}
