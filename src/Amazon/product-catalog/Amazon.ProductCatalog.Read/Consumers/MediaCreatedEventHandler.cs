using Amazon.ProductCatalog.Read.Services;
using Amazon.SharedKernel.Media.Events;
using MassTransit;

namespace Amazon.ProductCatalog.Read.Consumers;

public class MediaCreatedEventHandler(ICatalogReadServices _readServices) : IConsumer<MediaCreatedEvent>
{
    public async Task Consume(ConsumeContext<MediaCreatedEvent> context)
    {
        var message = context.Message;
        await _readServices.UpdateImagePathAsync(message.OwnerId, message.FilePath);

    }
}
