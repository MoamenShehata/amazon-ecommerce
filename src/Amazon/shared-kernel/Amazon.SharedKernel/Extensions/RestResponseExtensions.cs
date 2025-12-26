using Amazon.SharedKernel.API;

namespace Amazon.SharedKernel.Extensions;

public static class RestResponseExtensions
{
    public static RestResponse<TDestination> MapTo<TSource, TDestination>(this RestResponse<TSource> response, TDestination destination)
    {
        return new RestResponse<TDestination>(
            destination,
            response.StatusCode,
            response.Error,
            response.Exception
        );
    }
}