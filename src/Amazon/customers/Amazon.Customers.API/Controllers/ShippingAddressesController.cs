using Amazon.Customers.Application;
using Amazon.Customers.Application.Dtos;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Customers.API.Controllers;

[Route("api/Customers/{customerId}/[controller]")]
public class ShippingAddressesController(CustomerAppService _customerAppService) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateShippingAddress(Guid customerId, [FromBody] CreateShippingAddressRequest request)
    {
        var result = await _customerAppService.CreateShippingAddressAsync(customerId, request);
        if (result.IsSuccess)
            return Ok(result.Value);

        return RestResult(result);
    }
}