using Amazon.Orders.Infrastructure.Data;
using Amazon.SharedKernel.Common.Services;
using Amazon.SharedKernel.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SDKs.Repository.Extensions;

namespace Amazon.Shipping.Infrastructure;

public static class InfrastructureDependencyRegistrar
{
    public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddGenericRepos()
            .AddBaseContext<ShippingContext>(o => o.UseSqlServer(configuration.GetConnectionString("Default")));
        ;
        return services;
    }
}
