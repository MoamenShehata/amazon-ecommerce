using Amazon.Customers.Application;
using Amazon.Customers.Application.Dtos;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Customers.API.Controllers;

[Authorize(Policy = "CUSTOMERS_POLICY")]
[Route("api/customers/me/[controller]")]
public class ShippingAddressesController(CustomerAppService _customerAppService) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateShippingAddress([FromBody] CreateShippingAddressRequest request)
    {
        var result = await _customerAppService.CreateShippingAddressAsync(UserId, request);
        if (result.IsSuccess)
            return Ok(result.Value);

        return RestResult(result);
    }
}