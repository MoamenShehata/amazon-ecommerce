using Amazon.Orders.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SDKs.Repository.Extensions;

namespace Amazon.Orders.Infrastructure
{
    public static class ApplicationDependencyRegistrar
    {
        public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddDbContext<OrdersContext>(op => op.UseSqlServer(configuration.GetConnectionString("Orders")))
                .AddGenericRepos()
                .AddBaseContext<OrdersContext>()
                ;

            //services.AddMassTransit(config =>
            //{
            //    config.SetKebabCaseEndpointNameFormatter();

            //    config.AddConsumers(typeof(ApplicationDependencyRegistrar).Assembly);

            //    config.UsingRabbitMq((ctxt, configurator) =>
            //    {
            //        configurator.Host(new Uri(configuration["MessageBroker:Host"]), host =>
            //        {
            //            host.Username(configuration["MessageBroker:User"]);
            //            host.Password(configuration["MessageBroker:Password"]);
            //        });
            //        configurator.ConfigureEndpoints(ctxt);
            //    });
            //});

            return services;
        }

    }
}
