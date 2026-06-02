namespace Amazon.Cart.Infrastructure.Integrations.Customers;

public class CustomersRestClient(
    HttpClient _httpClient
    ) : ICustomersIntegrationClient
{
    private const string myProfileRequestPath = "customers/me";

    public async Task<HttpContent> GetCurrentLoggedInCustomerProfileAsync() => await SendGetRequest(myProfileRequestPath);
    public async Task<HttpContent> GetCurrentLoggedInCustomerPaymentCardAsync(int cardId) => await SendGetRequest($"{myProfileRequestPath}/paymentCards/{cardId}");


    private async Task<HttpContent> SendGetRequest(string requestPath) => (await _httpClient.GetAsync(requestPath)).Content;
}
