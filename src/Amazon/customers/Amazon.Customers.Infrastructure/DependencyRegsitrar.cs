using Amazon.Customers.Application.CustomerProfiles;
using Amazon.Customers.Domain;
using Amazon.Customers.Infrastructure.Data;
using Amazon.Customers.Infrastructure.Integrations;
using Amazon.Customers.Infrastructure.Integrations.Lookups;
using Amazon.Customers.Infrastructure.Services;
using Amazon.SharedKernel.Http;
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
            .AddMemoryCache()
            .AddGenericRepos()
            .AddBaseContext<CustomersContext>(op => op.UseSqlServer(configuration.GetConnectionString("Customers")))
            .AddDbContext<CustomerReadContext>(op => op.UseSqlServer(configuration.GetConnectionString("Customers")))
            ;

        services
            .AddScoped<ICustomerProfileService, CustomerProfileService>()
            .AddScoped<IAddressService, AddressService>()
            ;

        services
            .AddHttpClient<ILookupsIntegrationClient, LookupsRestClient>(x => x.BaseAddress = new Uri(configuration.GetValue<string>("Services:Lookups")))
            .AddHttpMessageHandler<HttpClientErrorHandler>()
            ;

        return services;
    }

}