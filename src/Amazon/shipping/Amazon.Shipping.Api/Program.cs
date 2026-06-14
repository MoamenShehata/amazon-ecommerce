using Amazon.Orders.Infrastructure.Data;
using Amazon.SharedKernel.Extensions;
using Amazon.Shipping.Application;
using Amazon.Shipping.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
            .RegisterSharedServices(builder.Configuration)
            .RegisterApplicationDependencies(builder.Configuration)
            .RegisterInfrastructureDependencies(builder.Configuration)
            .AddJwtAuthentication(builder.Configuration)
            .AddSharedJobs();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var ctx = scope.ServiceProvider.GetRequiredService<ShippingContext>();
await ctx.Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
