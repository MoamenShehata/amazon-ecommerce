using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Amazon.SharedKernel.Jobs.RabbitMq;

public class IntegrationEventsPublishJob(
        ILogger<IntegrationEventsPublishJob> _logger,
        IServiceScopeFactory _serviceScopeFactory
        ) : BackgroundJobBase(_logger)
{
    protected override TimeSpan Interval => TimeSpan.FromSeconds(5);
    protected override async Task DoAsync(CancellationToken stoppingToken)
    {
        var scope = _serviceScopeFactory.CreateScope();
        var publisher = scope.ServiceProvider.GetRequiredService<EventsPublishService>();
        await publisher.PublishAsync();
    }
}