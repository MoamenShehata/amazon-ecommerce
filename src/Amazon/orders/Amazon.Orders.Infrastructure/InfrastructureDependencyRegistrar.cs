using Amazon.Orders.Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SDKs.Repository.Extensions;

namespace Amazon.Orders.Infrastructure
{
    public static class InfrastructureDependencyRegistrar
    {
        public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                //.AddDbContext<OrdersContext>(o => o.UseSqlServer(configuration.GetConnectionString("CatalogDatabase")))
                .AddGenericRepos()
                .AddBaseContext<OrdersContext>(op => op.UseSqlServer(configuration.GetConnectionString("Orders")))
                ;

            return services;
        }

    }
}
