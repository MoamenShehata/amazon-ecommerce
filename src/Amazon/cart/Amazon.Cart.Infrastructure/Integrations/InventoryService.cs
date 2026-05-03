using Amazon.Cart.Domain;

namespace Amazon.Cart.Infrastructure.Integrations;

internal class InventoryService : IInventoryService
{
    public async Task<bool> IsProductAvailableAsync(Guid productId)
    {
        return await Task.FromResult(false);
    }
}
