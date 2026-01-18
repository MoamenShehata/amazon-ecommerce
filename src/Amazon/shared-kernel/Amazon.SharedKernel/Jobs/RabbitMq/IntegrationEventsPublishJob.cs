using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Amazon.SharedKernel.Jobs.RabbitMq;

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
            var publisher = scope.ServiceProvider.GetRequiredService<EventsPublishService>();

            await publisher.PublishAsync();

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }

        _logger.LogInformation("ProductIntegrationEventsPublishJob stopped");
    }
}