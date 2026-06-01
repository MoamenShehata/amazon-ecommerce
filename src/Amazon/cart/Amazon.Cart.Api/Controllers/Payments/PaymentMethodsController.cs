using Amazon.Cart.Application.Payments;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Cart.Api.Controllers.Payments;

public class PaymentMethodsController(PaymentsAppService _paymentsAppService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return RestResult(await _paymentsAppService.GetPaymentMethodsAsync());
    }
}
