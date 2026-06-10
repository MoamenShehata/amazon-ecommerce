using Amazon.Cart.Domain.Payments;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Cart.Api.Seed;

public static class DataSeeder
{
    public static async Task SeedAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var repo = scope.ServiceProvider.GetRequiredService<IRepository<PaymentMethod, Guid>>();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        if ((await repo.GetAllAsync()).Count() == 0)
        {
            repo.Add(PaymentMethod.ForCash());
            repo.Add(PaymentMethod.ForVisa());
            repo.Add(PaymentMethod.ForStripe());
        }
    }
}