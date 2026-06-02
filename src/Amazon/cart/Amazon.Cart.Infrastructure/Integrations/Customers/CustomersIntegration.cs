using Amazon.Cart.Domain.Integrations.Customers;
using Amazon.Cart.Domain.Integrations.Customers.Dtos;
using Amazon.Cart.Infrastructure.Integrations.Customers.Adapters;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace Amazon.Cart.Infrastructure.Integrations.Customers;

public record PaymentCardDto(int Id, string CardHolder, string OriginalNumber, string MaskedNumber, int ExpiryMonth, int ExpiryYear);

public class CustomersIntegration(ICustomersIntegrationClient _customersClient) : ICustomersIntegration
{
    public async Task<RestResponse<CustomerDeliveryAddress>> GetDeliveryAddressOrDefaultAsync(int? addressId)
    {
        return await TryExecuteAsync(async () =>
         {
             var jsonResponse = await _customersClient.GetCurrentLoggedInCustomerProfileAsync();

             var address = await CustomerAddressAdapter.FromProfileResponseAsync(jsonResponse, addressId);

             return address is null
                ? RestResponse<CustomerDeliveryAddress>.NotFound("Address was not found")
                : RestResponse<CustomerDeliveryAddress>.Success(address);
         });
    }

    public async Task<RestResponse<CustomerPaymentCard>> GetPaymentCardAsync(int cardId)
    {
        return await TryExecuteAsync(async () =>
        {
            var jsonResponse = await _customersClient.GetCurrentLoggedInCustomerPaymentCardAsync(cardId);

            var paymentCard = await jsonResponse.ReadFromJsonAsync<PaymentCardDto>();

            return paymentCard is null
                ? RestResponse<CustomerPaymentCard>.NotFound("Payment card not found")
                : RestResponse<CustomerPaymentCard>.Success(new CustomerPaymentCard(paymentCard.OriginalNumber, paymentCard.MaskedNumber, paymentCard.ExpiryMonth, paymentCard.ExpiryYear));
        });
    }

    private async Task<RestResponse<T>> TryExecuteAsync<T>(Func<Task<RestResponse<T>>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            return RestResponse<T>.Failure(ex);
        }
    }
}
