using Amazon.SharedKernel.Media.Events;
using MassTransit;

namespace Amazon.ProductCatalog.Application.Products.Consumers;

public class MediaCreatedEventHandler(ProductsAppService _productsAppService) : IConsumer<MediaCreatedEvent>
{
    public async Task Consume(ConsumeContext<MediaCreatedEvent> context)
    {
        var message = context.Message;
        await _productsAppService.UpdateImagePathAsync(message.OwnerId, message.FilePath);

    }
}
