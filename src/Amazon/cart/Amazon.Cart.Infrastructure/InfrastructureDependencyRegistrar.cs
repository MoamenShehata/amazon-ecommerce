using Amazon.Cart.Domain.Integrations;
using Amazon.Cart.Domain.Integrations.Customers;
using Amazon.Cart.Domain.Integrations.Orders;
using Amazon.Cart.Domain.Services;
using Amazon.Cart.Infrastructure.Data;
using Amazon.Cart.Infrastructure.Integrations;
using Amazon.Cart.Infrastructure.Integrations.Customers;
using Amazon.Cart.Infrastructure.Integrations.Customers.Adapters;
using Amazon.Cart.Infrastructure.Integrations.Orders;
using Amazon.Cart.Infrastructure.Services;
using Amazon.SharedKernel.Common.Services;
using Amazon.SharedKernel.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SDKs.Repository.Extensions;

namespace Amazon.Cart.Infrastructure;

public static class InfrastructureDependencyRegistrar
{
    public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddGenericRepos()
            .AddBaseContext<ShoppingCartContext>(o => o.UseSqlServer(configuration.GetConnectionString("Default")));
        ;

        services.AddHttpClient()
            .AddHttpContextAccessor()
            ;

        services.AddHttpClient<ICustomersIntegrationClient, CustomersRestClient>(x => x.BaseAddress = new Uri(configuration.GetValue<string>("Services:Customers")))
            .AddHttpMessageHandler<HttpClientErrorHandler>()
            .AddHttpMessageHandler<InjectJwtFromCurrentSessionHandler>()
            ;

        services.AddHttpClient<OrdersIntegrationClient>(x => x.BaseAddress = new Uri(configuration.GetValue<string>("Services:Orders")))
            .AddHttpMessageHandler<HttpClientErrorHandler>()
            .AddHttpMessageHandler<InjectJwtFromCurrentSessionHandler>()
            ;

        services
            .AddScoped<IInventoryService, InventoryService>()
            .AddScoped<IOrdersIntegration, OrderIntegration>()
            .AddScoped<ICustomersIntegration, CustomersIntegration>()
            .AddScoped<IUserClaimsStore, CartUserClaimsStore>()
            .AddScoped<IPaymentCardsService, PaymentCardsService>()
            .AddScoped<PaymentCardAdapter>()
            ;

        return services;
    }
}
