using Amazon.Cart.Domain.Integrations;
using Amazon.SharedKernel.API;

namespace Amazon.Cart.Infrastructure.Integrations;

public class PaymentCardsService : IPaymentCardsService
{
    public async Task<RestResponse> CanSatisfyAmountAsync(CustomerPaymentCard value, string cvv, decimal amount)
    {
        // just dummy for now
        return await Task.FromResult(RestResponse.Success());
    }
}