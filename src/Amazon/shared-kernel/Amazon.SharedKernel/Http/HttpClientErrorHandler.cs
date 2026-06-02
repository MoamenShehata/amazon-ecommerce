namespace Amazon.SharedKernel.Http;

public class HttpClientErrorHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            return response;
        }
        catch (Exception ex)
        {
            // logger
            throw;
        }
    }
}