using Stripe;
using Stripe.Checkout;

namespace Amazon.Cart.Application.Payments.Stripe;

internal static class StripePaymentGatewayExtensions
{
    internal static PaymentStatus ExtractPaymentStatus(this Event callbackEvent)
    {
        if (callbackEvent.Type == "checkout.session.completed")
            return PaymentStatus.Paid;

        if (callbackEvent.Type == "payment_intent.payment_failed")
            return PaymentStatus.Failed_Insufficient;

        return PaymentStatus.Unknown;
    }
    
    internal static string ExtractCheckoutSessionId(this Event callbackEvent)
    {
        var session =
                    callbackEvent.Data.Object as Session;

        return session!.Id;
    }
}