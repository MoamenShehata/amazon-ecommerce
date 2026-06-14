using Amazon.Orders.Api;
using Amazon.Orders.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.AddServices();

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

var app = builder.Build();

using var scope = app.Services.CreateScope();
var ctx = scope.ServiceProvider.GetRequiredService<OrdersContext>();
await ctx.Database.MigrateAsync();

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
