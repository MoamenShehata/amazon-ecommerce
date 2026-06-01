using Amazon.Cart.Domain.Integrations;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Amazon.Cart.Infrastructure.Integrations;

internal class GetCustomerProfileResponse
{
    public Guid CustomerId { get; set; }
    public List<CustomerDeliveryAddress> Addresses { get; set; }
}

public record PaymentCardDto(int Id, string CardHolder, string OriginalNumber, string MaskedNumber, int ExpiryMonth, int ExpiryYear);

public class CustomerService(
    IHttpClientFactory _httpClientFactory,
    IHttpContextAccessor _httpContextAccessor) : ICustomerService
{
    public async Task<RestResponse<CustomerDeliveryAddress>> GetCustomerDeliveryAddressOrDefaultAsync(Guid userId, int? addressId)
    {
        using var client = _httpClientFactory.CreateClient();

        var accessToken = AccessToken();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        try
        {
            var response = await client.GetAsync($"https://localhost:7128/api/customers/me");

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadFromJsonAsync<GetCustomerProfileResponse>();
            var address = responseBody.Addresses.FirstOrDefault(a => (addressId.HasValue && a.Id == addressId) || true);

            return RestResponse<CustomerDeliveryAddress>.Success(address);
        }
        catch (Exception ex)
        {
            return RestResponse<CustomerDeliveryAddress>.Failure(ex);
        }
    }

    public async Task<RestResponse<CustomerPaymentCard>> GetPaymentCardAsync(Guid userId, int cardId)
    {
        using var client = _httpClientFactory.CreateClient();
        var accessToken = AccessToken();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        try
        {
            var response = await client.GetAsync($"https://localhost:7128/api/customers/me/paymentCards/{cardId}");
            response.EnsureSuccessStatusCode();

            var paymentCard = await response.Content.ReadFromJsonAsync<PaymentCardDto>();

            return paymentCard is null
                ? RestResponse<CustomerPaymentCard>.NotFound("Payment card not found")
                : RestResponse<CustomerPaymentCard>.Success(new CustomerPaymentCard(paymentCard.OriginalNumber, paymentCard.MaskedNumber, paymentCard.ExpiryMonth, paymentCard.ExpiryYear));
        }
        catch (Exception ex)
        {
            return RestResponse<CustomerPaymentCard>.Failure(ex);
        }
    }

    private string AccessToken()
    {
        var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers.Authorization.ToString();
        //var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

        var accessToken = string.Empty;

        if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            accessToken = authorizationHeader["Bearer ".Length..].Trim();
        }

        return accessToken;
    }
}
