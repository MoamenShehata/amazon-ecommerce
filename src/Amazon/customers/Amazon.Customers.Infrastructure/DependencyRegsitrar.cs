using Amazon.Customers.Application.CustomerProfiles;
using Amazon.Customers.Domain;
using Amazon.Customers.Infrastructure.Data;
using Amazon.Customers.Infrastructure.Integrations;
using Amazon.Customers.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SDKs.Repository.Extensions;

namespace Amazon.Customers.Infrastructure;

public static class InfrastructureDependencyRegistrar
{
    public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddGenericRepos()
            .AddBaseContext<CustomersContext>(op => op.UseSqlServer(configuration.GetConnectionString("Customers")))
            .AddBaseContext<CustomerReadContext>(op => op.UseSqlServer(configuration.GetConnectionString("Customers")))
            ;

        services
            .AddScoped<ICustomerProfileService, CustomerProfileService>()
            .AddScoped<IAddressService, AddressService>()
            ;

        return services;
    }

}