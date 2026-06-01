using Amazon.Customers.Application;
using Amazon.Customers.Application.Dtos;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Customers.API.Controllers;

[Authorize]
[Route("api/customers/me/[controller]")]
public class PaymentCardsController(CustomerAppService _customerAppService) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentCardRequest request)
    {
        var result = await _customerAppService.CreatePaymentCardAsync(UserId, request);
        if (result.IsSuccess)
            return Ok(result.Value);

        return RestResult(result);
    }


    [HttpGet("{cardId}")]
    public async Task<IActionResult> GetById(int cardId)
    {
        return RestResult(await _customerAppService.GetPaymentCardAsync(UserId, cardId));
    }
}