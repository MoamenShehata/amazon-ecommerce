using Amazon.Cart.Domain.Integrations;
using Amazon.Cart.Domain.Services;
using Amazon.Cart.Infrastructure.Data;
using Amazon.Cart.Infrastructure.Integrations;
using Amazon.Cart.Infrastructure.Services;
using Amazon.SharedKernel.Common.Services;
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
            .AddScoped<ICustomerService, CustomerService>()
            .AddScoped<IUserClaimsStore, CartUserClaimsStore>()
            ;

        return services;
    }
}
