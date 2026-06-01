using Amazon.Customers.Application.CustomerProfiles.Models;
using Amazon.Customers.Application.Dtos;

namespace Amazon.Customers.Application.CustomerProfiles;

public interface ICustomerProfileService
{
    Task<CustomerProfile> GetByIdAsync(Guid customerId);
    Task CreateAsync(CustomerProfile customerProfile);
    Task UpdateShippingAddressesAsync(Guid customerId, ICollection<CustomerProfileAddress> newAddresses);
    Task UpdatePaymentCardsAsync(Guid customerId, ICollection<PaymentCardDto> newCards);
}