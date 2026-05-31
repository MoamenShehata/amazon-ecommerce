namespace Amazon.Cart.Domain.Payments.ValueObjects;

public class PaymentRequestPayload
{
    internal string? Payload { get; private set; }
    internal bool IsConfirmed { get; private set; }

    internal PaymentRequestPayload(string? payload, bool isConfirmed)
    {
        Payload = payload;
        IsConfirmed = isConfirmed;
    }

    internal PaymentRequestPayload WithConfirmation() => new(Payload, true);
}