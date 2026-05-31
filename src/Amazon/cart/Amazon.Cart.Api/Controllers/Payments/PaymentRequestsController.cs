using Amazon.Cart.Api.Dtos;
using Amazon.Cart.Application.Payments;
using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Cart.Api.Controllers.Payments;

[Authorize(Policy = "CUSTOMERS_POLICY")]
[Route("api/paymentMethods/{paymentMethodId}/requests")]
public class PaymentRequestsController(PaymentsAppService _paymentsAppService) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreatePaymentRequest(Guid paymentMethodId, [FromBody] CreatePaymentRequestDto request)
    {
        var result = await _paymentsAppService.CreatePaymentRequestAsync(paymentMethodId, request.DeliverToAddressId);
        if (result.IsSuccess)
            return Ok(result.Value);

        return RestResult(result);
    }
}
