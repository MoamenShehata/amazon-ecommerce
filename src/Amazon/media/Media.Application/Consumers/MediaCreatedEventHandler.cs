using Amazon.SharedKernel.Media.Events;
using MassTransit;

namespace Media.Application.Consumers
{
    public class MediaCreatedEventHandler(MediaService _mediaService) : IConsumer<MediaCreateRequestedEvent>
    {
        public async Task Consume(ConsumeContext<MediaCreateRequestedEvent> context)
        {
            var @event = context.Message;

            await _mediaService.CreateAsync(@event.MediaId, @event.OwnerId, @event.Stream, @event.IsPublic);
        }
    }
}