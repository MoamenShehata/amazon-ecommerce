using Amazon.Cart.Application;
using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Application.Settings.PaymentGateways;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace Amazon.Cart.Api.Controllers;

[Route("api/carts/{cartId}/[controller]")]
public class CheckoutController(
    CartAppService _cartAppService,
    IOptions<PaymentGatewaySettings> paymentGatewayOptions) : ApiControllerBase
{
    private readonly PaymentGatewaySettings _paymentGatewaySettings = paymentGatewayOptions.Value;

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
            Request.Headers[_paymentGatewaySettings.Stripe.WebHookSecretHeaderName],
            _paymentGatewaySettings.Stripe.WebHookSecret);

        return RestResult(await _cartAppService.ProcessStripeCallbackAsync(stripeEvent));
    }
}