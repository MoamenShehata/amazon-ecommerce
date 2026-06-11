using Amazon.Cart.Application.Payments.Challenge.Handlers;
using Amazon.Cart.Application.Payments.Confirmation;
using Amazon.Cart.Domain.Payments;

namespace Amazon.Cart.Application.Payments;

public interface IPaymentMethodChallengeHandlerFactory
{
    Task<IPaymentMethodChallengeHandler> CreateForChallengeAsync(Guid paymentMethodId);
    IPaymentMethodConfirmationHanlder CreateForConfirmation(PaymentMehodType paymentMethodType);
}
