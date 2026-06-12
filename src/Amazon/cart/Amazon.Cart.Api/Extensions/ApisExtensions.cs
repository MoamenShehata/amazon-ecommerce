using Amazon.SharedKernel.API;
using System.Net;

namespace Amazon.Cart.Api.Extensions;

public static class ApisExtensions
{
    public static IResult RestResult<TValue>(RestResponse<TValue> restResponse)
    {
        switch (restResponse.StatusCode)
        {
            case HttpStatusCode.OK:
                return Results.Ok(restResponse.Value);

            case HttpStatusCode.BadRequest:
                return Results.BadRequest(restResponse.Error);

            case HttpStatusCode.NotFound:
                return Results.NotFound(restResponse.Error);

            case HttpStatusCode.Conflict:
                return Results.Conflict(restResponse.Error);

            case HttpStatusCode.InternalServerError:
                return Results.InternalServerError(restResponse.Error);

            default:
                return Results.Ok(restResponse.Value);
        }
    }

    public static IResult RestCreatedResult<TValue>(RestResponse<TValue> restResponse, string route, Func<TValue, object> routeValues)
    {
        if (!restResponse.IsSuccess)
            return RestResult(restResponse);

        return Results.CreatedAtRoute(route, routeValues(restResponse.Value), restResponse.Value);
    }
}