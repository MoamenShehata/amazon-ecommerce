using Amazon.Customers.Domain.ValueObjects;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;

namespace Amazon.Customers.Domain;

public class CustomerService(IRepository<Customer, Guid> _repository)
{
    public async Task<RestResponse<Customer>> CreateCustomerAsync(Guid id, string email, string phoneNumber)
    {
        var customerResult = await GetByIdAsync(id);
        if (customerResult.IsSuccess)
            return customerResult;

        var newCustomer = new Customer(id, new ContactInfo(email, phoneNumber));

        _repository.Add(newCustomer);
        return RestResponse<Customer>.Created(newCustomer, newCustomer.Id.ToString());
    }

    public async Task<RestResponse<bool>> CreateShippingAddressAsync(Guid customerId, CityInfo city, HouseInfo house, bool isDefault)
    {
        //var customerResult = await GetByIdAsync(customerId);
        //if (customerResult is null)
        //    return RestResponse<bool>.NotFound($"Customer with id {customerId} not found.");

        // validate cityinfo

        //customerResult.Value.
        //return new ShippingAddress(customerId, city, house, isDefault);
        return RestResponse<bool>.Success(true);
    }

    private async Task<RestResponse<Customer>> GetByIdAsync(Guid customerId)
    {
        var customer = await _repository.GetInstanceAsync(customerId);
        if (customer is null)
            return RestResponse<Customer>.NotFound($"Customer with id {customerId} not found.");

        return RestResponse<Customer>.Success(customer);
    }
}