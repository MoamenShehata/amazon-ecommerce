using System.ComponentModel.DataAnnotations;

namespace Amazon.Cart.Application.Settings.PaymentGateways;

public class StripeSettings
{
    public const string SectionName = "Stripe";

    [Required]
    public string WebHookSecretHeaderName { get; init; } = string.Empty;

    [Required]
    public string WebHookSecret { get; init; } = string.Empty;
}