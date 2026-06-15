using DnsClient.Internal;
using Microsoft.Extensions.Logging;

namespace Amazon.SharedKernel.Http;

public class HttpClientErrorHandler(ILogger<HttpClientErrorHandler> _logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["requestUrl"] = request.RequestUri,
        });

        try
        {
            _logger.LogDebug("Executing request {@request}", request);

            var response = await base.SendAsync(request, cancellationToken);

            _logger.LogDebug("Executed request, the response is {@response}", await response.Content.ReadAsStringAsync());

            response.EnsureSuccessStatusCode();

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing request {@request}", request);
            throw;
        }
    }
}