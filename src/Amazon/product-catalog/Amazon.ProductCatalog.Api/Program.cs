using Amazon.ProductCatalog.Api;
using Amazon.ProductCatalog.Api.Jobs;
using Amazon.ProductCatalog.Application;
using Amazon.ProductCatalog.Infrastructure;
using Amazon.ProductCatalog.Infrastructure.Data;
using Amazon.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.AddServices();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

using var scope = app.Services.CreateScope();
var ctx = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
await ctx.Database.MigrateAsync();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
