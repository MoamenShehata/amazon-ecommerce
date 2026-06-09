using Amazon.Cart.Application;
using Amazon.Cart.Application.Dtos;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Cart.Api.Controllers;

[Route("api/carts/{cartId}/[controller]")]
public class CheckoutController(CartAppService _cartAppService) : ApiControllerBase
{
    [Authorize(Policy = "CARTS_POLICY")]
    [HttpPost("CreateOrder")]
    public async Task<IActionResult> ChallengePaymentMethodAndCreateOrder(Guid cartId, [FromBody] ChallengePaymentRequest request)
    {
        return RestResult(await _cartAppService.ChallengePaymentAndCreateOrderAsync(cartId, request));
    }

    [Authorize(Policy = "CARTS_POLICY")]
    [HttpPost("ConfirmPayment")]
    public async Task<IActionResult> ConfirmPayment(Guid cartId, [FromBody] ConfirmPaymentRequest request)
    {
        return RestResult(await _cartAppService.ConfirmPaymentAsync(cartId, request));
    }
}