using Amazon.Cart.Domain;
using Amazon.Cart.Domain.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Amazon.Cart.Application;

public static class ApplicationDependencyRegistrar
{
    public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        AddDomainServices(services);
        AddApplicationServices(services);

        return services;
    }

    private static void AddDomainServices(IServiceCollection services)
    {
        services
            .AddScoped<ShoppingCartService>()
            .AddScoped<ShoppingCartFactory>()
            ;
    }

    private static void AddApplicationServices(IServiceCollection services)
    {
        services
            .AddScoped<CartService>()
            ;
    }
}
