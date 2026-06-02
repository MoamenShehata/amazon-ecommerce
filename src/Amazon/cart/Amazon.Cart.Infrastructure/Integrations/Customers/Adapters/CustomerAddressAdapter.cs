using Amazon.Cart.Domain.Integrations.Customers.Dtos;
using Amazon.Cart.Infrastructure.Integrations.Customers.Dtos;
using System.Net.Http.Json;

namespace Amazon.Cart.Infrastructure.Integrations.Customers.Adapters;

public class CustomerAddressAdapter
{
    public static async Task<CustomerDeliveryAddress> FromProfileResponseAsync(HttpContent profileResponse, int? addressId)
    {
        var customerProfile = await profileResponse.ReadFromJsonAsync<GetCustomerProfileResponse>();

        return customerProfile.Addresses.FirstOrDefault(a => (addressId.HasValue && a.Id == addressId) || true);
    }
}