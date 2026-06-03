using Amazon.Orders.Domain.Orders.ValueObjects.Status;
using FluentValidation;

namespace Amazon.Orders.Application.Orders.Validators;

public class DeliveryMemberValidator : AbstractValidator<DeliveryMember>
{
    public DeliveryMemberValidator()
    {
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}