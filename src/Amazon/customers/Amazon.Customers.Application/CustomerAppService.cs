using Amazon.Customers.Application.CustomerProfiles;
using Amazon.Customers.Application.CustomerProfiles.Models;
using Amazon.Customers.Domain;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Customers.Events;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Customers.Application;

public class CustomerAppService(
    CustomerService _customerService,
    IUnitOfWork _unitOfWork,
    ICustomerProfileService _profilesService)
{
    public async Task CreateCustomerAsync(NewCustomerRegistrationEvent customerData)
    {
        await _customerService.CreateCustomerAsync(customerData.Id, customerData.Email, customerData.PhoneNumber);
        await _unitOfWork.CommitAsync();
    }

    public async Task<RestResponse<CustomerProfile>> GetCustomerProfileAsync(Guid customerId)
    {
        var profile = await _profilesService.GetByIdAsync(customerId);
        if (profile is null)
            return RestResponse<CustomerProfile>.BadRequest($"Customer profile for id {customerId} was not found");

        return RestResponse<CustomerProfile>.Success(profile);
    }
}