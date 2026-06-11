using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Domain.Services;
using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;

namespace Amazon.Cart.Application.Payments.Challenge;

public class PaymentMethodChallengeStartegy(
    CartService _cartService,
    IPaymentMethodChallengeHandlerFactory factory
    )
{
    public async Task<RestResponse<ChallengePaymentResponse>> ChallengeCustomerAsync(ShoppingCart shoppingCart, Guid paymentMethodId, Guid customerId, int customerDeliverToAddressId)
    {
        var challengeHandler = await factory.CreateAsync(paymentMethodId);

        return await challengeHandler.HandleAsync(shoppingCart, customerId, customerDeliverToAddressId);
    }
}