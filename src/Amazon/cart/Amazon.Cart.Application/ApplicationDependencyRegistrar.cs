using Amazon.Cart.Application.Payments;
using Amazon.Cart.Application.Payments.Challenge;
using Amazon.Cart.Application.Payments.Challenge.Handlers;
using Amazon.Cart.Application.Payments.Confirmation;
using Amazon.Cart.Application.Payments.Stripe;
using Amazon.Cart.Domain.Factories;
using Amazon.Cart.Domain.Payments;
using Amazon.Cart.Domain.Payments.Factories;
using Amazon.Cart.Domain.Services;
using Amazon.Cart.Domain.Specifications;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Amazon.Cart.Application;

public static class ApplicationDependencyRegistrar
{
    public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services,
        IConfiguration configuration)
    {
        AddDomainServices(services);
        AddApplicationServices(services, configuration);

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

        return services;
    }

    private static void AddDomainServices(IServiceCollection services)
    {
        services
            .AddScoped<CartService>()
            .AddScoped<ShoppingCartFactory>()
            .AddScoped<PaymentsService>()
            .AddScoped<PaymentRequestPayloadFactory>()
            .AddScoped<ShoppingCartSpecification>()
            .AddScoped<ProductService>()
            ;
    }

    private static void AddApplicationServices(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<CartAppService>()
            .AddScoped<PaymentsAppService>()
            .AddSingleton(new Stripe.StripeClient(configuration.GetValue<string>("PaymentGateways:Stripe:Secret")))
            .AddScoped<PaymentMethodChallengeStartegy>()
            .AddScoped<StripeServices>()
            ;

        services
            .AddScoped<IPaymentMethodChallengeHandlerFactory, PaymentMethodChallengHandlerFactory>()
            .AddScoped<CashPaymentMethodHandler>()
            .AddScoped<VisaPaymentMethodHandler>()
            .AddSingleton<StripePaymentMethodHandler>()
            .AddScoped<CashConfirmationHanlder>()
            .AddScoped<VisaConfirmationHanlder>()
            ;
    }
}
