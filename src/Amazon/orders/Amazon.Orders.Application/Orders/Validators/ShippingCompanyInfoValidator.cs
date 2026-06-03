using Amazon.Orders.Domain.Orders.ValueObjects.Status;
using FluentValidation;

namespace Amazon.Orders.Application.Orders.Validators;

public class ShippingCompanyInfoValidator : AbstractValidator<ShippingCompanyInfo>
{
    public ShippingCompanyInfoValidator()
    {
        RuleFor(x => x.Address).NotEmpty();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Website).NotEmpty();
    }
}
