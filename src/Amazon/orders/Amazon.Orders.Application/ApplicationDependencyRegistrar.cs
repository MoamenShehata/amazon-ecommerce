using Amazon.Orders.Domain.Products;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Amazon.Orders.Application
{
    public static class ApplicationDependencyRegistrar
    {
        public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
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

            services
                .AddDomainServices();

            return services;
        }

        private static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<ProductsService>();

            return services;
        }
    }
}
