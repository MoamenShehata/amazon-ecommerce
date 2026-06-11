using Amazon.Cart.Domain.Integrations.Inventory;
using Amazon.Cart.Domain.ShoppingCarts.Entites;
using Amazon.Inventory.Grpc;
using Amazon.SharedKernel.API;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using static Amazon.Inventory.Grpc.ProductService;

namespace Amazon.Cart.Infrastructure.Integrations.Inventory;

internal class InventoryIntegration(IConfiguration _configuration) : IInventoryIntegration
{
    public async Task<RestResponse<bool>> IsProductAvailableForQuantityAsync(Guid productId, int quantity)
    {
        var client = CreateProductServiceClient();

        try
        {
            using var call = client.IsProductAvailableInStockAsync(new ProductAvailabilityRequest { ProductId = productId.ToString(), Quantity = quantity });
            var result = await call.ResponseAsync;

            if (!result.IsAvailableInStock)
                return RestResponse<bool>.Conflict($"Product with id {productId} is not available in inventory");

            return RestResponse<bool>.Success(result.IsAvailableInStock);
        }
        catch (Exception ex)
        {
            return RestResponse<bool>.Failure("Inventory service unavailable, canot check item availablity right now");
        }

    }

    private ProductServiceClient CreateProductServiceClient()
    {
        var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Services:Inventory"));
        return new ProductServiceClient(channel);
    }
}
