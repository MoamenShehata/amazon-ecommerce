using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Application.Payments.Validators;
using Amazon.Cart.Domain.Payments;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Common.Services;
using Amazon.SharedKernel.Orders.Events;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Cart.Application.Payments.Confirmation;

public interface IPaymentMethodConfirmationHanlder
{
    Task<RestResponse<CheckoutPaymentInfo>> ConfirmAsync(ConfirmPaymentRequest request, Guid customerId, decimal amount);
}

public class CashConfirmationHanlder(
    IOtpService _otpService,
    EventStoreService _eventStoreService
    ) : IPaymentMethodConfirmationHanlder
{
    public async Task<RestResponse<CheckoutPaymentInfo>> ConfirmAsync(ConfirmPaymentRequest request, Guid customerId, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(request.Otp))
            return RestResponse<CheckoutPaymentInfo>.BadRequest("Please provide a valid otp");

        var isOtpValid = await _otpService.ValidateAsync(customerId, request.Otp);
        if (!isOtpValid)
            return RestResponse<CheckoutPaymentInfo>.BadRequest($"Invalid otp {request.Otp}");

        return RestResponse<CheckoutPaymentInfo>.Success(new CashOnDeliveryCheckoutInfo());
    }
}

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