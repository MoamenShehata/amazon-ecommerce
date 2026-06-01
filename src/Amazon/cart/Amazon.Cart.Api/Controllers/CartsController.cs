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

    [Authorize(Policy = "CARTS_POLICY")]
    [HttpPut("{cartId}")]
    public async Task<IActionResult> SetupForCheckout(Guid cartId, [FromBody] UpdateCartDto request)
    {
        var result = await _cartService.SetupForCheckoutAsync(cartId, request);
        return RestResult(result);
    }

    [Authorize(Policy = "CARTS_POLICY")]
    [HttpPost("{cartId}/checkoutOtp")]
    public async Task<IActionResult> CheckoutCartUsingOtp(Guid cartId, [FromBody] CheckoutUsingOtpDto request)
    {
        var result = await _cartService.CheckoutCartUsingOtpAsync(cartId, request.Otp);
        if (result.IsSuccess)
            return Ok(result.Value);

        return RestResult(result);
    }

    [Authorize(Policy = "CARTS_POLICY")]
    [HttpPost("{cartId}/checkoutVisa")]
    public async Task<IActionResult> CheckoutCartUsingVisa(Guid cartId, [FromBody] CheckoutUsingVisaRequest request)
    {
        var result = await _cartService.CheckoutCartUsingVisaAsync(cartId, request);
        if (result.IsSuccess)
            return Ok(result.Value);

        return RestResult(result);
    }
}
