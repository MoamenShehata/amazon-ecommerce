using Amazon.ProductCatalog.Domain.Products;
using Amazon.SharedKernel.Media;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Amazon.ProductCatalog.Infrastructure.Integration;

internal class UploadFileResult
{
    public string Url { get; set; }
}

public class ProductMediaService(IHttpClientFactory _httpClientFactory) : IMediaService
{
    public async Task<string> UploadFileAsync(MediaContent uploadRequest)
    {
        // call the actual media service
        using var client = _httpClientFactory.CreateClient();

        var requestBody = new
        {
            OwnerId = Guid.NewGuid(),
            IsPublic = true,
            uploadRequest.Content,
            uploadRequest.MimeType,
            uploadRequest.Name,
        };

        var requestAsJson = JsonSerializer.Serialize(requestBody);

        try
        {
            var response = await client.PostAsync($"https://localhost:7255/files", new StringContent(requestAsJson, new MediaTypeHeaderValue("application/json")));
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadFromJsonAsync<UploadFileResult>();
            return responseBody.Url;
        }
        catch (Exception ex)
        {
            return string.Empty;
            //if it fails we can upload it on the server as a fallback and return it temprarily and create an event to fix it later
        }

    }
}