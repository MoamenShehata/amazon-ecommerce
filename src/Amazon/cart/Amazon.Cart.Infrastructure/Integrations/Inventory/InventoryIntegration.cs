using Amazon.Cart.Domain.Integrations.Inventory;
using Amazon.Inventory.Grpc;
using Amazon.SharedKernel.API;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static Amazon.Inventory.Grpc.ProductService;

namespace Amazon.Cart.Infrastructure.Integrations.Inventory;

internal class InventoryIntegration(IConfiguration _configuration,
    ILogger<InventoryIntegration> _logger) : IInventoryIntegration
{
    public async Task<RestResponse<bool>> IsProductAvailableForQuantityAsync(Guid productId, int quantity)
    {
        var client = CreateProductServiceClient();

        try
        {
            using var call = client.IsProductAvailableInStockAsync(new ProductAvailabilityRequest { ProductId = productId.ToString(), Quantity = quantity }, new Grpc.Core.CallOptions(deadline: DateTime.UtcNow.AddSeconds(10)));
            var result = await call.ResponseAsync;

            if (!result.IsAvailableInStock)
                return RestResponse<bool>.Conflict($"Product with id {productId} is not available in inventory");

            return RestResponse<bool>.Success(result.IsAvailableInStock);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "checking for cart item Availablity failed {productId}", productId);
            return RestResponse<bool>.Failure("Inventory service unavailable, canot check item availablity right now");
        }

    }

    private ProductServiceClient CreateProductServiceClient()
    {
        var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Services:Inventory"), new GrpcChannelOptions
        {
            HttpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            }
        });
        return new ProductServiceClient(channel);
    }
}
