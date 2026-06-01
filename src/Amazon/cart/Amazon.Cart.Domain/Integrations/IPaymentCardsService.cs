using Amazon.SharedKernel.API;

namespace Amazon.Cart.Domain.Integrations;

public interface IPaymentCardsService
{
    Task<RestResponse> CanSatisfyAmountAsync(CustomerPaymentCard value, string cvv, decimal amount);
}