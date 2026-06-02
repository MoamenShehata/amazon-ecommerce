using Amazon.Cart.Domain.Integrations.Customers.Dtos;

namespace Amazon.Cart.Infrastructure.Integrations.Customers.Dtos;

internal class GetCustomerProfileResponse
{
    public List<CustomerDeliveryAddress> Addresses { get; set; } = [];
}