using Amazon.SharedKernel.API;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.Orders.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult RestResult<TValue>(RestResponse<TValue> restResponse)
    {
        switch (restResponse.StatusCode)
        {
            case System.Net.HttpStatusCode.OK:
                return Ok(restResponse.Value);

            case System.Net.HttpStatusCode.BadRequest:
                return BadRequest(restResponse.Error);

            case System.Net.HttpStatusCode.NotFound:
                return NotFound(restResponse.Error);

            case System.Net.HttpStatusCode.Conflict:
                return Conflict(restResponse.Error);

            case System.Net.HttpStatusCode.InternalServerError:
                return Problem(restResponse.Error.ToString());

            default:
                return Ok();
        }

    }
}
