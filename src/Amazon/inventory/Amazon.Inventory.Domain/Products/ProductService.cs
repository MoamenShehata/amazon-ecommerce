using Amazon.SharedKernel.Orders.Commands;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Inventory.Domain.Products;

public class ProductService(
    IRepository<Product, Guid> _products,
    EventStoreService _eventStore,
    IUnitOfWork _unitOfWork
    )
{
    public async Task ReserveProductItemsForOrderAsync(Guid orderId, List<KeyValuePair<Guid, int>> productsWithQuantities)
    {
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