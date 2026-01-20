using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Amazon.SharedKernel.Jobs;

public abstract class BackgroundJobBase(
        ILogger _logger
        ) : BackgroundService
{
    protected abstract Task DoAsync(CancellationToken stoppingToken);
    protected virtual TimeSpan Interval => TimeSpan.FromSeconds(10);

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var concreteJobType = GetType().Name;

        _logger.LogInformation($"{concreteJobType} started");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Background work running at {time}", DateTime.UtcNow);

            await DoAsync(stoppingToken);

            await Task.Delay(Interval, stoppingToken);
        }

        _logger.LogInformation($"{concreteJobType} stopped");
    }
}
