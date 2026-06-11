using Amazon.Cart.Application.Payments.Challenge.Handlers;

namespace Amazon.Cart.Application.Payments;

public interface IPaymentMethodChallengeHandlerFactory
{
    Task<IPaymentMethodChallengeHandler> CreateAsync(Guid paymentMethodId);
}
