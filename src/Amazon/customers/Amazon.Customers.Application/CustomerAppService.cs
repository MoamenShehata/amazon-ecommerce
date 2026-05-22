using Amazon.Customers.Application.CustomerProfiles;
using Amazon.Customers.Application.CustomerProfiles.Models;
using Amazon.Customers.Domain;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Customers.Events;
using Amazon.SharedKernel.Extensions;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Customers.Application;

public class CustomerAppService(
    CustomerService _customerService,
    IUnitOfWork _unitOfWork,
    ICustomerProfileService _profilesService,
    ShippingAddresAdapter _shippingAddresAdapter)
{
    public async Task CreateCustomerAsync(NewCustomerRegistrationEvent customerData)
    {
        await _customerService.CreateCustomerAsync(customerData.Id, customerData.Email, customerData.PhoneNumber);
        await _unitOfWork.CommitAsync();
    }

    public async Task CreateProfileForCustomerAsync(Guid customerId)
    {
        var customerResult = await _customerService.GetByIdAsync(customerId);
        if (!customerResult.IsSuccess) return;

        var existingProfile = await _profilesService.GetByIdAsync(customerId);
        if (existingProfile != null) return;

        var addressesResult = await _shippingAddresAdapter.ToReadModel(customerResult.Value.ShippingInfo);
        if (!addressesResult.IsSuccess) return; // or get one by one and check if exists, or return error to nack back to broker

        var profile = new CustomerProfile()
        {
            CustomerId = customerId,
            Addresses = addressesResult.Value
        };

        await _profilesService.CreateAsync(profile);
    }

    public async Task<RestResponse<CustomerProfile>> GetCustomerProfileAsync(Guid customerId)
    {
        var profile = await _profilesService.GetByIdAsync(customerId);
        if (profile is null)
            return RestResponse<CustomerProfile>.BadRequest($"Customer profile for id {customerId} was not found");

        return RestResponse<CustomerProfile>.Success(profile);
    }
}