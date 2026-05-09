using Amazon.Inventory.Domain.Products;
using Amazon.SharedKernel.IntegrationEvents.ShoppingCart;
using MassTransit;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Inventory.Application.Products.EventConsumers;
public class CartItemAddedEventHandler(
IRepository<Product, Guid> _productsRepo,
IUnitOfWork _unitOfWork) : IConsumer<CartItemAddedEvent>
{
    public async Task Consume(ConsumeContext<CartItemAddedEvent> context)
    {
        var @event = context.Message;

        var product = await _productsRepo.GetInstanceAsync(@event.ProductId);
        if (product is null) return;

        var consumeResult = product.Inventory.Consume(1);

        await _unitOfWork.CommitAsync();
    }
}
