using System.ComponentModel.DataAnnotations;

namespace Amazon.Cart.Application.Settings.PaymentGateways;

public class PaymentGatewaySettings
{
    public const string SectionName = "PaymentGateways";

    [Required]
    public StripeSettings Stripe { get; set; }
}
