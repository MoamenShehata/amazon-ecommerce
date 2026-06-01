using Amazon.SharedKernel.API;

namespace Amazon.Cart.Domain.Integrations;

public interface IPaymentCardsService
{
    Task<RestResponse> TryChargeAmountAsync(CustomerPaymentCard value, string cvv, decimal amount);
}