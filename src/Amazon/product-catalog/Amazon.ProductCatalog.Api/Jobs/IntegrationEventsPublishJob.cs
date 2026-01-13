using System.Reflection;
using System.Text.Json;
using Amazon.ProductCatalog.Application.Categories;
using Amazon.SharedKernel.IntegrationEvents.Products;
using MassTransit;
using MassTransit.Transports;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.ProductCatalog.Api.Jobs;

public class IntegrationEventsPublishJob(
        ILogger<IntegrationEventsPublishJob> _logger,
        IServiceScopeFactory _serviceScopeFactory
        ) : BackgroundService
{
    private Dictionary<string, Type> _mappers = new()
    {
        {"Amazon.ProductCatalog.Domain.Products.Events.ProductCreatedEvent, Amazon.ProductCatalog.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",typeof(ProductCreatedIntegrationEvent) }
    };

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ProductIntegrationEventsPublishJob started");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Background work running at {time}", DateTime.UtcNow);

            var scope = _serviceScopeFactory.CreateScope();
            var eventStoreService = scope.ServiceProvider.GetRequiredService<EventStoreService>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            foreach (var item in await eventStoreService.GetAllPendingAsync())
            {
                try
                {
                    var success = _mappers.TryGetValue(item.Type, out Type eventType);
                    if (!success) continue;

                    var integrationEvent = JsonSerializer.Deserialize(item.Body, eventType);

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