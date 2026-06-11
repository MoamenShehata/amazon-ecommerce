namespace Amazon.Cart.Application.Payments.Stripe;

internal enum PaymentStatus
{
    Paid = 0,
    Failed_Insufficient = 1,
    Unknown = 100,
}
