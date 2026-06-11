using Amazon.Cart.Application.Dtos;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Common.Services;
using Amazon.SharedKernel.Orders.Events;

namespace Amazon.Cart.Application.Payments.Confirmation;

public class CashConfirmationHanlder(
    IOtpService _otpService
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
