using Amazon.ProductCatalog.Api.Jobs;
using Amazon.ProductCatalog.Application;
using Amazon.ProductCatalog.Infrastructure;
using Amazon.SharedKernel.Extensions;
using Microsoft.OpenApi.Models;

namespace Amazon.ProductCatalog.Api;

public static class DependencyRegistrar
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();


        builder.Services
            .AddSharedJobs()
            .AddJob<CategoriesSoftDeleteJob>()
            ;

        builder.Services.AddCors(op =>
        {
            op.AddPolicy("CORS_Policy", policy =>
            {
                policy.WithOrigins("http://localhost:4200", "http://localhost:62832")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Amazon Product Catalog API",
                Version = "v1",
                Description = "API for product catalog management"
            });
        });

        builder.Services
            .RegisterApplicationDependencies(builder.Configuration)
            .RegisterInfrastructureDependencies(builder.Configuration)
            .AddSharedJobs();

    }
}