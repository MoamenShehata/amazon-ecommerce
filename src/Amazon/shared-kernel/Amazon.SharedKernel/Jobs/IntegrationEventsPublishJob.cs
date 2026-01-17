using System.Text.Json;
using Amazon.SharedKernel.IntegrationEvents.Orders;
using Amazon.SharedKernel.IntegrationEvents.Products;
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
    private Dictionary<string, Type> _mappers = new()
    {
        {"Amazon.ProductCatalog.Domain.Products.Events.ProductCreatedEvent, Amazon.ProductCatalog.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",typeof(ProductCreatedIntegrationEvent) },
        {"Amazon.Orders.Domain.Orders.Events.OrderCreatedEvent, Amazon.Orders.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",typeof(OrderCreatedIntegrationEvent) }
    };

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