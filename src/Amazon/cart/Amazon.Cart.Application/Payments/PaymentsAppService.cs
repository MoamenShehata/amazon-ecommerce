using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Application.Mappers;
using Amazon.Cart.Application.Payments.Dtos;
using Amazon.Cart.Domain.Integrations.Customers;
using Amazon.Cart.Domain.Payments;
using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Common.Services;
using Amazon.SharedKernel.Extensions;
using Microsoft.Extensions.Configuration;
using Moamen.SDKs.Repository;
using Stripe.Checkout;

namespace Amazon.Cart.Application.Payments
{
    public class PaymentsAppService(
        IRepository<PaymentMethod, Guid> _paymentMethods,
        IOtpService _otpService,
        ISmsService _smsService,
        ICustomersIntegration _customerIntegration,
        IConfiguration _configuration,
        Stripe.StripeClient _stripeClient
        )
    {
        public async Task<RestResponse<List<PaymentMethodDto>>> GetPaymentMethodsAsync()
        {
            var methods = await _paymentMethods.GetAllAsync();
            return RestResponse<List<PaymentMethodDto>>.Success(methods.Select(m => new PaymentMethodDto(m.Id, m.Name)).ToList());
        }

        public async Task<RestResponse<ChallengePaymentResponse>> ChallengePaymentAsync(ShoppingCart cart, Guid orderId, Guid paymentMethodId, Guid customerId, int deliveryToCustomerAddressId)
        {
            var paymentMethod = await _paymentMethods.GetInstanceAsync(paymentMethodId);
            if (paymentMethod is null)
                return RestResponse<ChallengePaymentResponse>.NotFound(new { Message = $"Payment method with id {paymentMethodId} not found" });

            var frontEndBaseUrl = _configuration.GetValue<string>("Front_Url");

            var deliveryAddressResult = await _customerIntegration.GetDeliveryAddressOrDefaultAsync(deliveryToCustomerAddressId);
            if (!deliveryAddressResult.IsSuccess)
                return deliveryAddressResult.MapTo(null as ChallengePaymentResponse);

            switch (paymentMethod.Type)
            {
                // we should check if he already came here before, and he is just fooling around
                case PaymentMehodType.Cash:
                    var otp = await _otpService.GenerateAsync(customerId);
                    await _smsService.SendMessageAsync(deliveryAddressResult.Value.PhoneNumber, $"Your OTP for confirming your cash payment is: {otp}");
                    return RestResponse<ChallengePaymentResponse>.Success(new ChallengePaymentResponse($"{frontEndBaseUrl}/cart/checkout/cash", PaymentMehodType.Cash));

                case PaymentMehodType.Visa:
                    return RestResponse<ChallengePaymentResponse>.Success(new ChallengePaymentResponse($"{frontEndBaseUrl}/cart/checkout/card", PaymentMehodType.Visa));

                case PaymentMehodType.Stripe:
                    return await CreateStripeSessionAsync(cart);

                default:
                    return RestResponse<ChallengePaymentResponse>.Failure(new NotSupportedException("Payment method is not supported currently!"));
            }
        }

        private async Task<RestResponse<ChallengePaymentResponse>> CreateStripeSessionAsync(ShoppingCart cart)
        {
            if (!string.IsNullOrWhiteSpace(cart.CheckedoutSessionId))
                return GetCheckoutSessionResponse(await _stripeClient.V1.Checkout.Sessions.GetAsync(cart.CheckedoutSessionId));

            var options = new SessionCreateOptions
            {
                LineItems = cart.ToSessionLineItems(),
                Mode = "payment",
                SuccessUrl = $"{_configuration.GetValue<string>("Front_Url")}/my/orders/{cart.OrderId}",
            };

            Session session = await _stripeClient.V1.Checkout.Sessions.CreateAsync(options);
            cart.SetCheckedoutSession(session.Id);

            return GetCheckoutSessionResponse(session);
        }

        private RestResponse<ChallengePaymentResponse> GetCheckoutSessionResponse(Session session)
        {
            return RestResponse<ChallengePaymentResponse>.Success(new ChallengePaymentResponse(session.Url, PaymentMehodType.Stripe));
        }
    }
}