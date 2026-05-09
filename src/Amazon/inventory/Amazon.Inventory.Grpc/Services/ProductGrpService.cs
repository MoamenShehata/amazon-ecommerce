using Amazon.Inventory.Application.Products;
using Grpc.Core;
using static Amazon.Inventory.Grpc.ProductService;

namespace Amazon.Inventory.Grpc.Services;

public class ProductGrpService(ProductAppService _service) : ProductServiceBase
{
    public override async Task<ProductAvailabilityReply> IsProductAvailableInStock(ProductAvailabilityRequest request, ServerCallContext context)
    {
        var productInstance = await _service.GetByIdAsync(Guid.Parse(request.ProductId));
        if (!productInstance.IsSuccess)
            return new ProductAvailabilityReply()
            {
                IsAvailableInStock = false,
                AvailableQuantity = 0
            };

        return new ProductAvailabilityReply()
        {
            IsAvailableInStock = productInstance.Value.Inventory.InStockCount >= request.Quantity,
            AvailableQuantity = productInstance.Value.Inventory.InStockCount
        };
    }

    public override async Task<HoldItemForPurchaseReply> HoldItemForPurchaseRequest(ProductIdRequest request, ServerCallContext context)
    {
        var productId = Guid.Parse(request.ProductId);

        var productInstance = await _service.GetByIdAsync(productId);
        if (!productInstance.IsSuccess)
            throw new RpcException(new Status(StatusCode.NotFound, $"Product with id {productId} not found"));

        var holdResult = await _service.HoldItemForPurchaseAsync(productId);
        if (!holdResult.IsSuccess)
            throw new RpcException(new Status(StatusCode.FailedPrecondition, holdResult.Error.ToString()!));

        return new HoldItemForPurchaseReply()
        {
            PurchaseRequestId = holdResult.Value
        };
    }
}