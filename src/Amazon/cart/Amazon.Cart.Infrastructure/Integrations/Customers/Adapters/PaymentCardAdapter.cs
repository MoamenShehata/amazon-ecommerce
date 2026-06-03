using Amazon.SharedKernel.Common.Services;
using System.Net.Http.Json;

namespace Amazon.Cart.Infrastructure.Integrations.Customers.Adapters;

public class PaymentCardAdapter(ITextServices _textServices)
{
    public async Task<PaymentCardDto> FromResponseAsync(HttpContent response)
    {
        var paymentCard = await response.ReadFromJsonAsync<PaymentCardDto>();

        var cardNumberDecrypted = await _textServices.DecryptAsync(paymentCard.OriginalNumber);

        return paymentCard with { OriginalNumber = cardNumberDecrypted };
    }
}