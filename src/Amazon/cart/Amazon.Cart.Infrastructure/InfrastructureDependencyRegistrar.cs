using Amazon.Cart.Domain.Integrations;
using Amazon.Cart.Domain.Integrations.Customers;
using Amazon.Cart.Domain.Integrations.Orders;
using Amazon.Cart.Domain.Payments;
using Amazon.Cart.Domain.Products;
using Amazon.Cart.Domain.Services;
using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.Cart.Domain.ShoppingCarts.Entites;
using Amazon.Cart.Infrastructure.Data.Models;
using Amazon.Cart.Infrastructure.Integrations;
using Amazon.Cart.Infrastructure.Integrations.Customers;
using Amazon.Cart.Infrastructure.Integrations.Customers.Adapters;
using Amazon.Cart.Infrastructure.Integrations.Orders;
using Amazon.Cart.Infrastructure.Services;
using Amazon.SharedKernel.Common.Services;
using Amazon.SharedKernel.Extensions;
using Amazon.SharedKernel.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Amazon.Cart.Infrastructure;

public static class InfrastructureDependencyRegistrar
{
    public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        ConfigureMongoEntities();

        services.AddScoped<EventStoreService>();

        services
           .AddMongoDb(configuration.GetConnectionString("Mongo"), "amazonCarts")
           .AddMongoRepo<ShoppingCart, Guid>("carts")
           .AddMongoRepo<Product, Guid>("products")
           .AddMongoRepo<PaymentMethod, Guid>("paymentMethods")
           .AddMongoRepo<OutboxMessage, Guid>("outboxMessages")
           .AddMongoRepo<CustomerClaim, ObjectId>("customerClaims")
           ;

        services.AddScoped<IUnitOfWork, MongoDbUnitOfWork>();

        services.AddHttpClient()
            .AddHttpContextAccessor()
            ;

        services.AddHttpClient<ICustomersIntegrationClient, CustomersRestClient>(x => x.BaseAddress = new Uri(configuration.GetValue<string>("Services:Gateway")))
            .AddHttpMessageHandler<HttpClientErrorHandler>()
            .AddHttpMessageHandler<InjectJwtFromCurrentSessionHandler>()
            ;

        services.AddHttpClient<OrdersIntegrationClient>(x => x.BaseAddress = new Uri(configuration.GetValue<string>("Services:Gateway")))
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

    private static void ConfigureMongoEntities()
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        BsonClassMap.RegisterClassMap<ShoppingCart>(cm =>
        {
            cm.AutoMap();

            cm.MapField("_cartItems");
        });
    }
}
