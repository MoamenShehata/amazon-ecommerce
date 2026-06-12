using Amazon.Cart.Api;
using Amazon.Cart.Api.Seed;
using Amazon.Cart.Application.Payments;
using static Amazon.Cart.Api.Extensions.ApisExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiServices(builder.Configuration);

var app = builder.Build();

await app.Services.SeedAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/api/paymentMethods", async (PaymentsAppService _paymentsAppService) => RestResult(await _paymentsAppService.GetPaymentMethodsAsync()));

app.Run();
