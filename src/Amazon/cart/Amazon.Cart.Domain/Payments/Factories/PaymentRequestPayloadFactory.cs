using Amazon.Cart.Domain.Payments.ValueObjects;
using System.Text.Json;

namespace Amazon.Cart.Domain.Payments.Factories;

public class PaymentRequestPayloadFactory
{
    public PaymentRequestPayload OfCash(string phoneNumber, string country, string city, string postalCode, string street, string buildingNumber)
    {
        var payload = new PayWithCashPayload(phoneNumber, country, city, postalCode, street, buildingNumber);

        return new PaymentRequestPayload(JsonSerializer.Serialize(payload), false);
    }

    internal static PaymentRequestPayload Empty => new EmptyPaymentRequestPayload();
}