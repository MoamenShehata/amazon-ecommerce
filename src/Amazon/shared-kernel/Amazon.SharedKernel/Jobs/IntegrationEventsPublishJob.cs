using System.Reflection;
using System.Text.Json;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Jobs;

public class IntegrationEventsPublishJob(
        ILogger<IntegrationEventsPublishJob> _logger,
        IServiceScopeFactory _serviceScopeFactory
        ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ProductIntegrationEventsPublishJob started");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Background work running at {time}", DateTime.UtcNow);

            var scope = _serviceScopeFactory.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var eventStoreService = scope.ServiceProvider.GetRequiredService<EventStoreService>();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            foreach (var item in await eventStoreService.GetAllPendingAsync())
            {
                try
                {
                    var ass = Assembly.GetExecutingAssembly();
                    var t = ass.GetType(item.Type);
                    var integrationEvent = JsonSerializer.Deserialize(item.Body, t);

                    await publishEndpoint.Publish(integrationEvent);
                    item.MarkAsSent();
                }
                catch (Exception ex)
                {
                    item.MarkAsFailed(ex.Message);
                }
            }

            await unitOfWork.CommitAsync();
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }

        _logger.LogInformation("ProductIntegrationEventsPublishJob stopped");
    }
}