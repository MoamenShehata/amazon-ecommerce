using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Domain.Integrations.Customers;
using Amazon.Cart.Domain.Payments;
using Amazon.Cart.Domain.Services;
using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Common.Services;
using Amazon.SharedKernel.Extensions;
using Microsoft.Extensions.Configuration;
using Moamen.SDKs.Repository;

namespace Amazon.Cart.Application.Payments.Challenge.Handlers;

public class CashPaymentMethodHandler(
    IOtpService _otpService,
    ISmsService _smsService,
    ICustomersIntegration _customerIntegration,
    IConfiguration _configuration
    ) : IPaymentMethodChallengeHandler
{
    public async Task<RestResponse<ChallengePaymentResponse>> HandleAsync(ShoppingCart shoppingCart, Guid customerId, int customerDeliverToAddressId)
    {
        var frontEndBaseUrl = _configuration.GetValue<string>("Front_Url");

        var deliveryAddressResult = await _customerIntegration.GetDeliveryAddressOrDefaultAsync(customerDeliverToAddressId);
        if (!deliveryAddressResult.IsSuccess)
            return deliveryAddressResult.MapTo(null as ChallengePaymentResponse);

        var otp = await _otpService.GenerateAsync(customerId);
        await _smsService.SendMessageAsync(deliveryAddressResult.Value.PhoneNumber, $"Your OTP for confirming your cash payment is: {otp}");
        
        shoppingCart.SetPaymentMethod(PaymentMehodType.Cash);

        return RestResponse<ChallengePaymentResponse>.Success(new ChallengePaymentResponse($"{frontEndBaseUrl}/cart/checkout/cash", PaymentMehodType.Cash));
    }
}
