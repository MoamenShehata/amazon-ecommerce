using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Application.Mappers;
using Amazon.Cart.Domain.Payments;
using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.SharedKernel.API;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

namespace Amazon.Cart.Application.Payments.Challenge.Handlers;

public class StripePaymentMethodHandler(
    IConfiguration _configuration,
    StripeClient _stripeClient
    ) : IPaymentMethodChallengeHandler
{
    public async Task<RestResponse<ChallengePaymentResponse>> HandleAsync(ShoppingCart shoppingCart, Guid customerId, int customerDeliverToAddressId)
    {
        shoppingCart.SetPaymentMethod(PaymentMehodType.Stripe);

        return await CreateStripeSessionAsync(shoppingCart);
    }

    private async Task<RestResponse<ChallengePaymentResponse>> CreateStripeSessionAsync(ShoppingCart cart)
    {
        if (!string.IsNullOrWhiteSpace(cart.CheckedoutSessionId))
            return GetCheckoutSessionResponse(await _stripeClient.V1.Checkout.Sessions.GetAsync(cart.CheckedoutSessionId));

        var options = new SessionCreateOptions
        {
            LineItems = cart.ToSessionLineItems(),
            Mode = "payment",
            SuccessUrl = $"{_configuration.GetValue<string>("Front_Url")}/my/orders/{cart.OrderId}",
        };

        Session session = await _stripeClient.V1.Checkout.Sessions.CreateAsync(options);
        cart.SetCheckedoutSession(session.Id);

        return GetCheckoutSessionResponse(session);
    }

    private RestResponse<ChallengePaymentResponse> GetCheckoutSessionResponse(Session session)
    {
        return RestResponse<ChallengePaymentResponse>.Success(new ChallengePaymentResponse(session.Url, PaymentMehodType.Stripe));
    }
}