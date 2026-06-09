using Amazon.Cart.Domain.Payments;

namespace Amazon.Cart.Application.Dtos;

public class ChallengePaymentRequest
{
    public int DeliverToAddressId { get; set; }
    public Guid PaymentMethodId { get; set; }
}

public record ChallengePaymentResponse(string RedirectUrl, PaymentMehodType PaymentMehod);

public class ConfirmPaymentRequest
{
    public string? Otp { get; set; }
    public CheckoutUsingVisaRequest? VisaDetails { get; set; }
}
