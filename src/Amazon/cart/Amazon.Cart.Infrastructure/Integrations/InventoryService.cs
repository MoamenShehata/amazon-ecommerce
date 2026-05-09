using Amazon.Cart.Domain;
using Amazon.Inventory.Grpc;
using Grpc.Net.Client;
using static Amazon.Inventory.Grpc.ProductService;

namespace Amazon.Cart.Infrastructure.Integrations;

internal class InventoryService : IInventoryService
{
    public async Task<bool> IsProductAvailableForQuantityAsync(Guid productId, int quantity)
    {
        var client = CreateProductServiceClient();

        using var call = client.IsProductAvailableInStockAsync(new ProductAvailabilityRequest { ProductId = productId.ToString(), Quantity = quantity });
        var result = await call.ResponseAsync;

        return await Task.FromResult(result.IsAvailableInStock);
    }

    public async Task<int> TryHoldProductItemForPurchaseAsync(Guid productId)
    {
        var client = CreateProductServiceClient();

        using var call = client.HoldItemForPurchaseRequestAsync(new ProductIdRequest { ProductId = productId.ToString() });
        var result = await call.ResponseAsync;

        return result.PurchaseRequestId;
    }

    private static ProductServiceClient CreateProductServiceClient()
    {
        var channel = GrpcChannel.ForAddress("https://localhost:7047");
        return new ProductServiceClient(channel);
    }
}
