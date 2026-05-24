using Amazon.Cart.Domain;
using Amazon.Cart.Infrastructure.Data;
using Amazon.Cart.Infrastructure.Integrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SDKs.Repository.Extensions;

namespace Amazon.Cart.Infrastructure;

public static class InfrastructureDependencyRegistrar
{
    public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddGenericRepos()
            .AddBaseContext<ShoppingCartContext>(o => o.UseSqlServer(configuration.GetConnectionString("Default")));
        ;

        services.AddHttpClient();

        services
            .AddScoped<IInventoryService, InventoryService>()
            .AddScoped<IOrderService, OrderService>()
            ;

        return services;
    }
}
