using Amazon.Cart.Application.Payments.Challenge.Handlers;
using Amazon.Cart.Application.Payments.Confirmation;
using Amazon.Cart.Domain.Payments;
using Moamen.SDKs.Repository;
using Stripe;

namespace Amazon.Cart.Application.Payments;

public class PaymentMethodChallengHandlerFactory(
    IRepository<Domain.Payments.PaymentMethod, Guid> _paymentMethods,
    CashPaymentMethodHandler _cashChallengeHandler,
    VisaPaymentMethodHandler _visaChallengeHandler,
    StripePaymentMethodHandler _stripeChallengeHandler,

    CashConfirmationHanlder _cashConfirmationHanlder,
    VisaConfirmationHanlder _visaConfirmationHanlder
    ) : IPaymentMethodChallengeHandlerFactory
{
    public async Task<IPaymentMethodChallengeHandler> CreateForChallengeAsync(Guid paymentMethodId)
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

    public IPaymentMethodConfirmationHanlder CreateForConfirmation(PaymentMehodType paymentMethodType)
    {
        return paymentMethodType switch
        {
            PaymentMehodType.Cash => _cashConfirmationHanlder,
            PaymentMehodType.Visa => _visaConfirmationHanlder,
            _ => throw new NotImplementedException()
        };
    }
}
