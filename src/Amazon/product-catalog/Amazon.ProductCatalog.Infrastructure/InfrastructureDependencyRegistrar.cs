using Amazon.ProductCatalog.Domain.Products;
using Amazon.ProductCatalog.Infrastructure.Data;
using Amazon.ProductCatalog.Infrastructure.Integration;
using Amazon.SharedKernel.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moamen.SDKs.Repository.Extensions;

namespace Amazon.ProductCatalog.Infrastructure
{
    public static class InfrastructureDependencyRegistrar
    {
        public static IServiceCollection RegisterInfrastructureDependencies(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddGenericRepos()
                .AddBaseContext<CatalogDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("CatalogDatabase")));
            ;

            services
            .AddHttpClient<MediaRestClient>(x => x.BaseAddress = new Uri(configuration.GetValue<string>("Services:Gateway")))
            .ConfigurePrimaryHttpMessageHandler(() =>
                new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                })
            .AddHttpMessageHandler<HttpClientErrorHandler>()
            ;

            services
                .AddScoped<IMediaService, ProductMediaService>()
                ;

            return services;
        }
    }
}
