using Amazon.SharedKernel.API;

namespace Amazon.Cart.Domain.Integrations.Inventory;

public interface IInventoryIntegration
{
    Task<RestResponse<bool>> IsProductAvailableForQuantityAsync(Guid productId, int quantity);
}
