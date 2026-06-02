using Amazon.SharedKernel.API;

namespace Amazon.Cart.Domain.Integrations;

public record CustomerDeliveryAddress(int Id, string PhoneNumber, string Country, string City, string PostalCode, string Street, int BuildingNumber);
public record CustomerPaymentCard(string OriginalNumber, string MaskedNumber, int ExpireyMonth, int ExpiryYear);

public interface ICustomersIntegration
{
    Task<RestResponse<CustomerDeliveryAddress>> GetDeliveryAddressOrDefaultAsync(int? addressId);
    Task<RestResponse<CustomerPaymentCard>> GetPaymentCardAsync(int cardId);
}