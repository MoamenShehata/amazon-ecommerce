using Amazon.Inventory.Domain.Products;
using Amazon.SharedKernel.IntegrationEvents.Products;
using Amazon.SharedKernel.Products.Events;
using MassTransit;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Inventory.Application.Products.EventConsumers;

public class InventoryProductCreatedEventHandler(
IRepository<Product, Guid> _productsRepo,
IUnitOfWork _unitOfWork) : IConsumer<ProductCreatedEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        var productEvent = context.Message;

        var product = new Product(productEvent.ProductId, productEvent.InStockCount);
        _productsRepo.Add(product);

        await _unitOfWork.CommitAsync();
    }
}
