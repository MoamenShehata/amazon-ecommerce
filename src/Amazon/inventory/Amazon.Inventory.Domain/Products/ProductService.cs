using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Orders.Commands;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Inventory.Domain.Products;

public class ProductService(
    IRepository<Product, Guid> _products,
    EventStoreService _eventStore
    )
{
    public async Task<RestResponse<Product>> IsProductInStockAsync(Guid productId)
    {
        var product = await _products.GetInstanceAsync(x => x.Id == productId && !x.IsDeleted);
        if (product == null)
            return RestResponse<Product>.NotFound($"Product with ID {productId} not found.");

        return RestResponse<Product>.Success(product);
    }

    public async Task ReserveProductItemsForOrderAsync(Guid orderId, List<KeyValuePair<Guid, int>> productsWithQuantities)
    {
        _eventStore.Append(new InventoryReservationFailedEvent(orderId, new()));
        return;
        var products = await _products.GetAllAsync(x => productsWithQuantities.Select(d => d.Key).Contains(x.Id));

        List<Guid> productOutOfStockIds = [];
        foreach (var product in products)
        {
            var reservationResult = product.ReserveQuantityForOrder(orderId, productsWithQuantities.FirstOrDefault(x => x.Key == product.Id).Value);
            if (!reservationResult.IsSuccess)
                productOutOfStockIds.Add(product.Id);
        }

        if (productOutOfStockIds.Any())
            _eventStore.Append(new InventoryReservationFailedEvent(orderId, productOutOfStockIds));
        else
            _eventStore.Append(new InventoryReservedEvent(orderId));
    }
}