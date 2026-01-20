using Amazon.Cart.Application;
using Amazon.Cart.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .RegisterApplicationDependencies(builder.Configuration)
    .RegisterInfrastructureDependencies(builder.Configuration)
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("CORS_Policy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
