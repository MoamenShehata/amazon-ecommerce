using Amazon.Customers.Application.CustomerProfiles;
using Amazon.Customers.Application.CustomerProfiles.Models;
using Amazon.Customers.Application.Dtos;
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

    public async Task UpdateProfileAddressesForCustomerAsync(Guid customerId)
    {
        var customerResult = await _customerService.GetByIdAsync(customerId);
        if (!customerResult.IsSuccess) return;

        var existingProfile = await _profilesService.GetByIdAsync(customerId);
        if (existingProfile is null) return;

        var addressesResult = await _shippingAddresAdapter.ToReadModel(customerResult.Value.ShippingInfo);
        if (!addressesResult.IsSuccess) return; // or get one by one and check if exists, or return error to nack back to broker

        await _profilesService.UpdateShippingAddressesAsync(customerId, addressesResult.Value);
    }

    public async Task UpdateProfilePaymentCardsAsync(Guid customerId)
    {
        var customerResult = await _customerService.GetByIdAsync(customerId);
        if (!customerResult.IsSuccess) return;

        var existingProfile = await _profilesService.GetByIdAsync(customerId);
        if (existingProfile is null) return;

        await _profilesService.UpdatePaymentCardsAsync(customerId, customerResult.Value.PaymentCards.Select(x => new PaymentCardDto(x.Id, x.Info.HolderName, x.Info.Number.Value, x.Info.Expiration.ToString())).ToList());
    }

    public async Task<RestResponse<CustomerProfile>> GetCustomerProfileAsync(Guid customerId)
    {
        var profile = await _profilesService.GetByIdAsync(customerId);
        if (profile is null)
            return RestResponse<CustomerProfile>.BadRequest($"Customer profile for id {customerId} was not found");

        return RestResponse<CustomerProfile>.Success(profile);
    }

    public async Task<RestResponse<bool>> CreateShippingAddressAsync(Guid customerId, CreateShippingAddressRequest request)
    {
        var createResult = await _customerService.AddShippingAddressAsync(customerId, request.City, request.House, request.IsDefault);
        if (!createResult.IsSuccess)
            return createResult.MapTo(false);

        await _unitOfWork.CommitAsync();
        return RestResponse<bool>.Success(true);
    }

    public async Task<RestResponse<PaymentCardDto>> CreatePaymentCardAsync(Guid customerId, CreatePaymentCardRequest request)
    {
        var createResult = await _customerService.CreatePaymentCardAsync(customerId, request.CardHolder, request.CardNumber, request.ExpiresAt);
        if (!createResult.IsSuccess)
            return createResult.MapTo(null as PaymentCardDto);

        await _unitOfWork.CommitAsync();

        return RestResponse<PaymentCardDto>.Success(new PaymentCardDto(createResult.Value.Id, createResult.Value.Info.HolderName, createResult.Value.Info.Number.Value, createResult.Value.Info.Expiration.ToString()));
    }
}