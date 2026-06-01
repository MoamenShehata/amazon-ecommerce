using Amazon.Customers.Application;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Customers.API.Controllers;

[Authorize]
[Route("api/[controller]/me")]
public class CustomersController(CustomerAppService _customerAppService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCustomerProfile()
    {
        var result = await _customerAppService.GetCustomerProfileAsync(UserId);
        if (result.IsSuccess)
            return Ok(result.Value);

        return RestResult(result);
    }
}
