using Amazon.Inventory.Infrastructure.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SDKs.Repository.Extensions;

namespace Amazon.Inventory.Infrastructure
{
    public static class InfrastructureDependencyRegistrar
    {
        public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddDbContext<InventoryContext>(op => op.UseSqlServer(configuration.GetConnectionString("Inventory")))
                .AddGenericRepos()
                .AddBaseContext<InventoryContext>()
                ;

            return services;
        }

    }
}
