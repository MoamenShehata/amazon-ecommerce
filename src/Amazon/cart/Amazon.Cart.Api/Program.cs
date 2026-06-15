using Amazon.Cart.Api;
using Amazon.Cart.Api.Seed;
using Amazon.Cart.Application;
using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Application.Payments;
using Microsoft.AspNetCore.Mvc;
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

var cartsApis = app.MapGroup("/api/carts")
    .RequireAuthorization();


cartsApis
    .MapGet("{cartId}", async (Guid cartId, CartAppService _cartService)
        => RestResult(await _cartService.GetByIdAsync(cartId)))
    .WithName("GetCartById")
    .RequireAuthorization();

cartsApis
    .MapPost("", async ([FromBody] CartCreateDto cartModel, CartAppService _cartService)
        => RestCreatedResult(await _cartService.CreateCartAsync(cartModel), "GetCartById", result => new { cartId = result.CartId }))
    .AllowAnonymous()
    ;

var cartItemsApis = cartsApis.MapGroup("{cartId}/items");

cartItemsApis.MapPost("", async (Guid cartId, [FromBody] CartItemCreateDto cartItem, CartAppService _cartService)
    => RestResult(await _cartService.AddItemToCartAsync(cartId, cartItem)));

cartItemsApis.MapDelete("{productId}", async (Guid cartId, Guid productId, CartAppService _cartService)
    => RestResult(await _cartService.RemoveItemFromCartAsync(cartId, productId)));

cartItemsApis.MapDelete("RemoveAllProductItems/{productId}", async (Guid cartId, Guid productId, [FromBody] CartItemCreateDto cartItem, CartAppService _cartService)
    => RestResult(await _cartService.RemoveAllProductItemsAsync(cartId, productId)));

app.Run();
