using Amazon.Identity.Presentation.Models;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Common;
using Amazon.SharedKernel.Customers.Events;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using VidGuard.Platform.Authentication.Pages.Account.RegisterCustomer;

namespace Amazon.Identity.Presentation.Services;

public class AccountService(
    UserManager<ApplicationUser> _usersManager,
    IPublishEndpoint _publishEndpoint
    )
{
    public async Task<RestResponse<RegisterCustomerResult>> RegisterCustomerAsync(RegisterCustomerModel customerModel)
    {
        if (await _usersManager.FindByNameAsync(customerModel.Email) != null)
            return RestResponse<RegisterCustomerResult>.Conflict("Username already exists");

        var customer = new ApplicationUser
        {
            UserName = customerModel.Email,
            Email = customerModel.Email,
            PhoneNumber = customerModel.PhoneNumber
        };

        var creationResult = await _usersManager.CreateAsync(customer, customerModel.Password);
        if (!creationResult.Succeeded)
            return RestResponse<RegisterCustomerResult>.BadRequest(creationResult.Errors.FirstOrDefault().Description);

        await _usersManager.AddToRoleAsync(customer, RoleNames.Customer);

        await _publishEndpoint.Publish(new NewCustomerRegistrationEvent(Guid.Parse(customer.Id), customer.Email, customer.PhoneNumber));

        var result = new RegisterCustomerResult
        {
            Id = Guid.Parse(customer.Id),
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber
        };

        return RestResponse<RegisterCustomerResult>.Created(result, customer.Id);
    }
}