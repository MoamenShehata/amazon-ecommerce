using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Amazon.SharedKernel.API;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult RestResult<TValue>(RestResponse<TValue> restResponse)
    {
        switch (restResponse.StatusCode)
        {
            case HttpStatusCode.OK:
                return Ok(restResponse.Value);

            case HttpStatusCode.BadRequest:
                return BadRequest(restResponse.Error);

            case HttpStatusCode.NotFound:
                return NotFound(restResponse.Error);

            case HttpStatusCode.Conflict:
                return Conflict(restResponse.Error);

            case HttpStatusCode.InternalServerError:
                return StatusCode((int)HttpStatusCode.InternalServerError, restResponse.Error);

            default:
                return Ok();
        }

    }

    public Guid UserId => Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
    public string UserEmail => User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
}
