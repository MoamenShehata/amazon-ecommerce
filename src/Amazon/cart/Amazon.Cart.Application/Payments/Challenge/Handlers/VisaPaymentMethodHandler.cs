using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Domain.Payments;
using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.SharedKernel.API;
using Microsoft.Extensions.Configuration;

namespace Amazon.Cart.Application.Payments.Challenge.Handlers;

public class VisaPaymentMethodHandler(IConfiguration _configuration) : IPaymentMethodChallengeHandler
{
    public Task<RestResponse<ChallengePaymentResponse>> HandleAsync(ShoppingCart shoppingCart, Guid customerId, int customerDeliverToAddressId)
    {
        var frontEndBaseUrl = _configuration.GetValue<string>("Front_Url");

        shoppingCart.SetPaymentMethod(PaymentMehodType.Visa);
        return Task.FromResult(RestResponse<ChallengePaymentResponse>.Success(new ChallengePaymentResponse($"{frontEndBaseUrl}/cart/checkout/card", PaymentMehodType.Visa)));
    }
}
