using Amazon.Orders.Application.Orders;
using Amazon.Orders.Domain.Orders;
using Amazon.Orders.Domain.Orders.Factories;
using Amazon.Orders.Domain.Products;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Amazon.Orders.Application
{
    public static class ApplicationDependencyRegistrar
    {
        public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddSerilog((services, lc) =>
                {
                    lc.ReadFrom.Configuration(configuration);
                    //lc.WriteTo.Seq("http://localhost:5341/",);
                });

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

            services.AddScoped<OrdersAppService>()
                ;

            return services;
        }

        private static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services
                .AddScoped<ProductsService>()
                .AddScoped<OrdersService>()
                .AddScoped<OrderFactory>()
                ;

            return services;
        }
    }
}
