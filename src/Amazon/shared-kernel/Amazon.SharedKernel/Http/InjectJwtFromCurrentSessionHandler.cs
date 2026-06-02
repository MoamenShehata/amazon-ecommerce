using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Amazon.SharedKernel.Http;

public class InjectJwtFromCurrentSessionHandler(IHttpContextAccessor _httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        InjectJwtToRequest(request);

        var response = await base.SendAsync(request, cancellationToken);

        return response;
    }

    private void InjectJwtToRequest(HttpRequestMessage request)
    {
        var jwt = GetAccessTokenFromCurrentSession();

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessTokenFromCurrentSession());
    }

    private string GetAccessTokenFromCurrentSession()
    {
        var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers.Authorization.ToString();
        //var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

        var accessToken = string.Empty;

        if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            accessToken = authorizationHeader["Bearer ".Length..].Trim();
        }

        return accessToken;
    }
}
