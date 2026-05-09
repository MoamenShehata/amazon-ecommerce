using Amazon.Customers.Domain.Entities;
using Amazon.Customers.Domain.ValueObjects;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;

namespace Amazon.Customers.Domain;

//public class CustomerService(IRepository<Customer, Guid> _repository)
public class CustomerService()
{
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

    //private async Task<RestResponse<Customer>> GetByIdAsync(Guid customerId)
    //{
    //    var customer = await _repository.GetInstanceAsync(customerId);
    //    if (customer is null)
    //        return RestResponse<Customer>.NotFound($"Customer with id {customerId} not found.");

    //    return RestResponse<Customer>.Success(customer);
    //}
}