using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Application.Payments.Validators;
using Amazon.Cart.Domain.Payments;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Orders.Events;

namespace Amazon.Cart.Application.Payments.Confirmation;

public class VisaConfirmationHanlder(
        PaymentsService _paymentsService
    ) : IPaymentMethodConfirmationHanlder
{
    public async Task<RestResponse<CheckoutPaymentInfo>> ConfirmAsync(ConfirmPaymentRequest request, Guid customerId, decimal amount)
    {
        var validationResult = new CheckoutUsingVisaRequestValidator().Validate(request.VisaDetails);
        if (!validationResult.IsValid)
            return RestResponse<CheckoutPaymentInfo>.BadRequest(validationResult.Errors.FirstOrDefault().ErrorMessage);

        var chargeCardResult = await _paymentsService.ChargePaymentCardForAmountAsync(customerId, request.VisaDetails.PaymentCardId, request.VisaDetails.Cvv, amount);
        if (!chargeCardResult.IsSuccess)
            return RestResponse<CheckoutPaymentInfo>.BadRequest(chargeCardResult.Error.ToString());

        return RestResponse<CheckoutPaymentInfo>.Success(new PaymentCardCheckoutInfo(request.VisaDetails.PaymentCardId, chargeCardResult.Value));
    }
}