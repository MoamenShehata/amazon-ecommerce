using Amazon.Cart.Domain;
using Amazon.Cart.Domain.Factories;
using MassTransit;
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

        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.AddConsumers(typeof(ApplicationDependencyRegistrar).Assembly);

            config.UsingRabbitMq((ctxt, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]), host =>
                {
                    host.Username(configuration["MessageBroker:User"]);
                    host.Password(configuration["MessageBroker:Password"]);
                });
                configurator.ConfigureEndpoints(ctxt);
            });
        });

        return services;
    }

    private static void AddDomainServices(IServiceCollection services)
    {
        services
            .AddScoped<CartService>()
            .AddScoped<ShoppingCartFactory>()
            ;
    }

    private static void AddApplicationServices(IServiceCollection services)
    {
        services
            .AddScoped<CartAppService>()
            ;
    }
}
