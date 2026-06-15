using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Amazon.ProductCatalog.Infrastructure.Integration;

internal record UploadFileRequest(Guid OwnerId, bool IsPublic, byte[] Content, string MimeType, string Name);

public class MediaRestClient(HttpClient _httpClient)
{
    private readonly string _basePath = "files";
    internal async Task<string> UploadFileAsync(UploadFileRequest request)
    {
        var requestAsJson = JsonSerializer.Serialize(request);

        var response = await _httpClient.PostAsync(_basePath, new StringContent(requestAsJson, new MediaTypeHeaderValue("application/json")));
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadFromJsonAsync<UploadFileResult>();
        return responseBody.Url;
    }
}