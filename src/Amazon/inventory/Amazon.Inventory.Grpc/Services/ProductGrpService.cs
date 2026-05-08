using Amazon.Inventory.Application.Products;
using Grpc.Core;
using static Amazon.Inventory.Grpc.ProductService;
namespace Amazon.Inventory.Grpc.Services;

public class ProductGrpService(ProductAppService _service) : ProductServiceBase
{
    public override async Task<ProductAvailabilityReply> IsProductAvailableInStock(ProductIdRequest request, ServerCallContext context)
    {
        var productInstance = await _service.GetByIdAsync(Guid.Parse(request.ProductId));
        if (productInstance is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Product with id {request.ProductId} not found"));


        return new ProductAvailabilityReply()
        {
            IsAvailableInStock = productInstance.Value.Inventory.InStockCount > 0,
            AvailableQuantity = productInstance.Value.Inventory.InStockCount
        };
    }
}