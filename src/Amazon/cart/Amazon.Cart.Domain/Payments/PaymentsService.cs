using Amazon.Cart.Domain.Integrations;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Common.Services;
using Amazon.SharedKernel.Extensions;
using Moamen.SDKs.Repository;

namespace Amazon.Cart.Domain.Payments;

public class PaymentsService(
    IRepository<PaymentMethod, Guid> _repository,
    IOtpService _otpService,
    ISmsService _smsService,
    ICustomersIntegration _customerService,
    IPaymentCardsService _paymentCardsService)
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

    public async Task<RestResponse<string>> TryWithdrawFromPaymentCardAsync(Guid customerId, int paymentCardId, string cvv, decimal amount)
    {
        var paymentCardResult = await _customerService.GetPaymentCardAsync(paymentCardId);
        if (!paymentCardResult.IsSuccess)
            return paymentCardResult.MapTo(string.Empty);

        // this is very simplified
        // we should talk to the card provider and acquire a transaction id, then we should asker the customer for the otp then hand both back to the provider to confirm the request then continue with the order
        var result = await _paymentCardsService.TryChargeAmountAsync(paymentCardResult.Value, cvv, amount);
        if (!result.IsSuccess)
            return RestResponse<string>.BadRequest(result.Error.ToString());

        return RestResponse<string>.Success(paymentCardResult.Value.MaskedNumber);
    }
}