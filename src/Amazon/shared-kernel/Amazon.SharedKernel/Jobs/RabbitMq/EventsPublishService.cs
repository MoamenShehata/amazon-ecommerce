using System.Reflection;
using System.Text.Json;
using MassTransit;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Jobs.RabbitMq
{
    public class EventsPublishService(
        EventStoreService eventStoreService,
        IPublishEndpoint publishEndpoint,
        IUnitOfWork unitOfWork)
    {
        public async Task PublishAsync()
        {
            foreach (var item in await eventStoreService.GetAllPendingAsync())
            {
                try
                {
                    var integrationEvent = JsonSerializer.Deserialize(item.Body, Assembly.GetExecutingAssembly().GetType(item.Type));

                    await publishEndpoint.Publish(integrationEvent);
                    item.MarkAsSent();
                }
                catch (Exception ex)
                {
                    item.MarkAsFailed(ex.Message);
                }
            }

            await unitOfWork.CommitAsync();
        }
    }
}