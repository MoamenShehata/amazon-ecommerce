using Amazon.Cart.Application;
using Amazon.SharedKernel.Jobs;

namespace Amazon.Cart.Api.Jobs;

public class PurgeExpiredCartsJob(
    ILogger<PurgeExpiredCartsJob> _logger,
    IServiceScopeFactory _serviceScopeFactory
    ) : BackgroundJobBase(_logger)
{
    protected override TimeSpan Interval => TimeSpan.FromSeconds(5);
    protected override async Task DoAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var cartService = scope.ServiceProvider.GetRequiredService<CartAppService>();
        await cartService.PurgeExpiredCartsAsync();
    }
}
