using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.SharedKernel.API;

namespace Amazon.Cart.Application.Payments.Challenge.Handlers;

public interface IPaymentMethodChallengeHandler
{
    Task<RestResponse<ChallengePaymentResponse>> HandleAsync(ShoppingCart shoppingCart, Guid customerId, int customerDeliverToAddressId);
}