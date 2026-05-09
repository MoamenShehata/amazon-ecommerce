using Amazon.Cart.Application;
using Amazon.SharedKernel.Jobs;
using Amazon.SharedKernel.Jobs.RabbitMq;

namespace Amazon.Cart.Api.Jobs;

public class SoftDeleteExpiredCartsJob(
    ILogger<SoftDeleteExpiredCartsJob> _logger,
    IServiceScopeFactory _serviceScopeFactory
    ) : BackgroundJobBase(_logger)
{
    protected override TimeSpan Interval => TimeSpan.FromSeconds(5);
    protected override async Task DoAsync(CancellationToken stoppingToken)
    {
        var scope = _serviceScopeFactory.CreateScope();
        var cartService = scope.ServiceProvider.GetRequiredService<CartService>();
        await cartService.SoftDeleteExpiredCartsAsync();
    }
}
