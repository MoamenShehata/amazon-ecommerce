using Amazon.Cart.Domain;
using Amazon.Inventory.Grpc;
using Grpc.Net.Client;
using static Amazon.Inventory.Grpc.ProductService;

namespace Amazon.Cart.Infrastructure.Integrations;

internal class InventoryService : IInventoryService
{
    public async Task<int> TryHoldProductItemForPurchaseAsync(Guid productId)
    {
        var channel = GrpcChannel.ForAddress("https://localhost:7047");
        var client = new ProductServiceClient(channel);

        using var call = client.HoldItemForPurchaseRequestAsync(new ProductIdRequest { ProductId = productId.ToString() });
        var result = await call.ResponseAsync;

        return result.PurchaseRequestId;
    }
}
