using Amazon.Cart.Domain.Integrations;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Common.Services;
using Moamen.SDKs.Repository;

namespace Amazon.Cart.Domain.Payments;

public class PaymentsService(
    IRepository<PaymentMethod, Guid> _repository,
    IOtpService _otpService,
    ISmsService _smsService)
{
    public async Task<RestResponse<int>> UsePaymentMethodAsync(Guid paymentMethodId, Guid userId, CustomerDeliveryAddress customerDeliveryAddress)
    {
        var paymentMethod = await _repository.GetInstanceAsync(paymentMethodId);
        if (paymentMethod is null)
            return RestResponse<int>.NotFound(new { Message = $"Payment method with id {paymentMethodId} not found" });

        switch (paymentMethod.Type)
        {
            case PaymentMehodType.Cash:
                var otp = await _otpService.GenerateAsync(userId);
                await _smsService.SendMessageAsync(customerDeliveryAddress.PhoneNumber, $"Your OTP for confirming your cash payment is: {otp}");
                return RestResponse<int>.Success((int)paymentMethod.Type);

            case PaymentMehodType.Visa:
                return RestResponse<int>.Success((int)paymentMethod.Type);

            default:
                throw new NotSupportedException();
        }
    }
}