using Media.Application;
using Media.Application.Storage;
using Media.Infrastructure;
using Amazon.SharedKernel.Extensions;
using Amazon.SharedKernel.Media;
using Media.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSharedJobs();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services
    .RegisterApplicationDependencies(builder.Configuration)
    .RegisterInfrastructureDependencies(builder.Configuration)
    ;

var app = builder.Build();

using var scope = app.Services.CreateScope();
var ctx = scope.ServiceProvider.GetRequiredService<MediaContext>();
await ctx.Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseResponseCaching();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
