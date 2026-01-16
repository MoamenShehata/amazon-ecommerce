using Amazon.Orders.Domain.Products;
using Amazon.SharedKernel.IntegrationEvents.Products;
using MassTransit;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Orders.Application.Products.Events.Consumers;

public class ProductCreatedEventHandler(
    ProductsService _productsService,
    IUnitOfWork _unitOfWork) : IConsumer<ProductCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context)
    {
        var message = context.Message;

        _productsService.Create(message.ProductId, message.Name, 50, message.UnitPrice);
        await _unitOfWork.CommitAsync();
    }
}