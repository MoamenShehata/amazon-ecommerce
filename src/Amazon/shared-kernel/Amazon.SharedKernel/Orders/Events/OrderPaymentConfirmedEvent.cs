using Moamen.SDKs.SharedKernel.DDD.Events;
using System.Text.Json.Serialization;

namespace Amazon.SharedKernel.Orders.Events;

public record OrderPaymentConfirmedEvent(Guid OrderId, CheckoutPaymentInfo PaymentInfo) : IntegrationEvent(DateTime.UtcNow);

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(CashOnDeliveryCheckoutInfo), "cod")]
[JsonDerivedType(typeof(PaymentCardCheckoutInfo), "card")]
[JsonDerivedType(typeof(PaymentGatewayCheckoutInfo), "gateway")]
public class CheckoutPaymentInfo
{
    public virtual List<KeyValuePair<string, string>> ToProps => new();
}

public class CashOnDeliveryCheckoutInfo : CheckoutPaymentInfo
{
    public override List<KeyValuePair<string, string>> ToProps => [new KeyValuePair<string, string>("Method", "Cash on delivery")];
}

public class PaymentCardCheckoutInfo : CheckoutPaymentInfo
{
    public PaymentCardCheckoutInfo(int id, string numberMasked)
    {
        Id = id;
        NumberMasked = numberMasked;
    }

    public int Id { get; set; }
    public string NumberMasked { get; set; }

    public override List<KeyValuePair<string, string>> ToProps => [
        new KeyValuePair<string, string>("Method", "Visa"),
        new KeyValuePair<string, string>("Number", NumberMasked)
        ];

}

public class PaymentGatewayCheckoutInfo : CheckoutPaymentInfo
{
    public PaymentGatewayCheckoutInfo(string sessionId)
    {
        SessionId = sessionId;
    }

    public string SessionId { get; set; }

    public override List<KeyValuePair<string, string>> ToProps => [new KeyValuePair<string, string>("Method", "Stripe gateway")];
}