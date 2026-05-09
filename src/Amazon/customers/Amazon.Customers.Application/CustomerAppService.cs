using Amazon.Customers.Domain;
using Amazon.SharedKernel.Customers.Events;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Customers.Application;

public class CustomerAppService(CustomerService _customerService,
    IUnitOfWork _unitOfWork)
{
    public async Task CreateCustomerAsync(NewCustomerRegistrationEvent customerData)
    {
        await _customerService.CreateCustomerAsync(customerData.Id, customerData.Email, customerData.PhoneNumber);
        await _unitOfWork.CommitAsync();
    }
}