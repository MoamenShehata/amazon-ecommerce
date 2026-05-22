using Amazon.Customers.Application.CustomerProfiles.Models;

namespace Amazon.Customers.Application.CustomerProfiles;

public interface ICustomerProfileService
{
    Task<CustomerProfile> GetByIdAsync(Guid customerId);
    Task CreateAsync(CustomerProfile customerProfile);
}