using Amazon.Orders.Domain.Products;
using Amazon.SharedKernel.Products.Events;
using MassTransit;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Orders.Application.Products.Events.Consumers;

public class ProductCreatedEventHandler(
    ProductsService _productsService,
    IUnitOfWork _unitOfWork) : IConsumer<ProductCreatedEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        var message = context.Message;

        _productsService.Create(message.ProductId, message.Name, message.InStockCount, message.UnitPrice);
        await _unitOfWork.CommitAsync();
    }
}
