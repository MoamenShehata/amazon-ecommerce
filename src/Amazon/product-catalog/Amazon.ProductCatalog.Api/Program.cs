using Amazon.ProductCatalog.Api.Jobs;
using Amazon.ProductCatalog.Application;
using Amazon.ProductCatalog.Infrastructure;
using Amazon.SharedKernel.Jobs;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHostedService<CategoriesSoftDeleteJob>();
builder.Services.AddHostedService<IntegrationEventsPublishJob>();

// Register OpenAPI + Swagger
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

// Existing app registrations
builder.Services
    .RegisterApplicationDependencies()
    .RegisterInfrastructureDependencies(builder.Configuration)
    ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Map OpenAPI document (existing)
    app.MapOpenApi();

    // Serve Swagger JSON and UI (dashboard) at /swagger
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Amazon Product Catalog API v1");
        options.RoutePrefix = "swagger"; // UI available at /swagger
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
