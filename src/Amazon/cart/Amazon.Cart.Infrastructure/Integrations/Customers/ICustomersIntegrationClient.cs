namespace Amazon.Cart.Infrastructure.Integrations.Customers;

public interface ICustomersIntegrationClient
{
    Task<HttpContent> GetCurrentLoggedInCustomerProfileAsync();
    Task<HttpContent> GetCurrentLoggedInCustomerPaymentCardAsync(int cardId);
}
