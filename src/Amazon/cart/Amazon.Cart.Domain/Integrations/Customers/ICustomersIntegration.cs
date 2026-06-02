using Amazon.Cart.Domain.Integrations.Customers.Dtos;
using Amazon.SharedKernel.API;

namespace Amazon.Cart.Domain.Integrations.Customers;

public interface ICustomersIntegration
{
    Task<RestResponse<CustomerDeliveryAddress>> GetDeliveryAddressOrDefaultAsync(int? addressId);
    Task<RestResponse<CustomerPaymentCard>> GetPaymentCardAsync(int cardId);
}