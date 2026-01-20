using Microsoft.AspNetCore.Mvc;

namespace Amazon.SharedKernel.API;

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

            default:
                return Ok();
        }

    }
}
