using Amazon.Cart.Application.Payments.Challenge.Handlers;
using Amazon.Cart.Domain.Payments;
using Moamen.SDKs.Repository;

namespace Amazon.Cart.Application.Payments;

public class PaymentMethodChallengHandlerFactory(
    IRepository<Domain.Payments.PaymentMethod, Guid> _paymentMethods,
    CashPaymentMethodHandler _cashChallengeHandler,
    VisaPaymentMethodHandler _visaChallengeHandler,
    StripePaymentMethodHandler _stripeChallengeHandler
    ) : IPaymentMethodChallengeHandlerFactory
{
    public async Task<IPaymentMethodChallengeHandler> CreateAsync(Guid paymentMethodId)
    {
        var paymentMethod = await _paymentMethods.GetInstanceAsync(paymentMethodId);
        if (paymentMethod is null) throw new Exception();

        return paymentMethod.Type switch
        {
            PaymentMehodType.Cash => _cashChallengeHandler,
            PaymentMehodType.Visa => _visaChallengeHandler,
            PaymentMehodType.Stripe => _stripeChallengeHandler,
            _ => throw new NotImplementedException()
        };
    }
}
