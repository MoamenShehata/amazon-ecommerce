using Amazon.Cart.Application;
using Amazon.Cart.Application.Dtos;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

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

    [Route("~/api/[controller]/StripeCallback")]
    [HttpPost]
    public async Task<IActionResult> StripeCallback()
    {
        var json = await new StreamReader(Request.Body)
            .ReadToEndAsync();

        var stripeEvent = EventUtility.ConstructEvent(
            json,
            Request.Headers["Stripe-Signature"],
            "whsec_z6vAQzg1kyTYaQiAicmnPS8PJCzvXSmV");

        if (stripeEvent.Type == "checkout.session.completed")
        {
            var session =
                stripeEvent.Data.Object as Session;

            var stripeSessionId = session!.Id;

            // Find Order by StripeSessionId

            // Mark Order Paid

            // Publish OrderPaid event
        }

        return Ok();
    }
}

[Route("api/[controller]")]
public class StripeController : ApiControllerBase
{

}