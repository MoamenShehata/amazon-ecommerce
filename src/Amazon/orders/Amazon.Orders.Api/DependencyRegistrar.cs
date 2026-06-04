using Amazon.Orders.Application;
using Amazon.Orders.Infrastructure;
using Amazon.SharedKernel.Extensions;

namespace Amazon.Orders.Api;

public static class DependencyRegistrar
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services
            .RegisterSharedServices(builder.Configuration)
            .RegisterApplicationDependencies(builder.Configuration)
            .RegisterInfrastructureDependencies(builder.Configuration)
            .AddJwtAuthentication(builder.Configuration)
            .AddSharedJobs();

    }
}