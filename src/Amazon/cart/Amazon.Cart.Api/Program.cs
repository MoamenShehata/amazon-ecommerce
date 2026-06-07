using Amazon.Cart.Api;
using Amazon.Cart.Domain;
using Amazon.Cart.Domain.Payments;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

BsonSerializer.RegisterSerializer(
    new GuidSerializer(GuidRepresentation.Standard));

BsonClassMap.RegisterClassMap<ShoppingCart>(cm =>
{
    cm.AutoMap();

    cm.MapField("_cartItems");
    var g = cm.MapField("_cartItems").Getter;
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddApiServices(builder.Configuration);

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

builder.Services.AddAuthorization(op =>
{
    op.AddPolicy("CARTS_POLICY", builder =>
    {
        builder.RequireClaim("scope", "amazon.cart");
    });
});


builder.Services.AddAuthorization(op =>
{
    op.AddPolicy("CUSTOMERS_POLICY", builder =>
    {
        builder.RequireClaim("scope", "amazon.customers");
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Use(async (ctxt, rd) =>
{
    var repo = ctxt.RequestServices.GetRequiredService<IRepository<PaymentMethod, Guid>>();
    var uow = ctxt.RequestServices.GetRequiredService<IUnitOfWork>();

    if ((await repo.GetAllAsync()).Count() == 0)
    {
        repo.Add(PaymentMethod.ForCash());
        repo.Add(PaymentMethod.ForVisa());
        //await uow.CommitAsync();
    }
    await rd(ctxt);
});
app.Run();
