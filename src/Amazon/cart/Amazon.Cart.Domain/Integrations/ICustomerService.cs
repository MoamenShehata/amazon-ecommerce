using Amazon.SharedKernel.API;

namespace Amazon.Cart.Domain.Integrations;

public record CustomerDeliveryAddress(int Id, string PhoneNumber, string Country, string City, string PostalCode, string Street, string BuildingNumber);

public interface ICustomerService
{
    Task<RestResponse<CustomerDeliveryAddress>> GetCustomerDeliveryAddressOrDefaultAsync(Guid userId, int? addressId);
}