using Amazon.ProductCatalog.Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SDKs.Repository.Extensions;

namespace Amazon.ProductCatalog.Infrastructure
{
    public static class InfrastructureDependencyRegistrar
    {
        public static void RegisterInfrastructureDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                //.AddDbContext<CatalogDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("CatalogDatabase")))
                .AddGenericRepos()
                //.AddOutboxServices()
                .AddBaseContext<CatalogDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("CatalogDatabase")));
            ;

            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                //if (subscribersAssembly != null)
                //    config.AddConsumers(subscribersAssembly);

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
        }
    }
}
