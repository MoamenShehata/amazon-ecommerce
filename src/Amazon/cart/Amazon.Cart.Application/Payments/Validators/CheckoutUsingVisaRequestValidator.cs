using Amazon.Cart.Application.Dtos;
using FluentValidation;

namespace Amazon.Cart.Application.Payments.Validators;

public class CheckoutUsingVisaRequestValidator : AbstractValidator<CheckoutUsingVisaRequest>
{
    public CheckoutUsingVisaRequestValidator()
    {
        RuleFor(x => x.PaymentCardId).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Cvv).NotEmpty();
    }
}
