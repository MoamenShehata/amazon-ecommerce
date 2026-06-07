using Amazon.Cart.Domain;
using Amazon.Cart.Domain.Factories;
using Amazon.SharedKernel.Data.NoSql;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

BsonSerializer.RegisterSerializer(
    new GuidSerializer(GuidRepresentation.Standard));

var _client = new MongoClient("mongodb://localhost:27017");
//var collection = _client.GetDatabase("amazonCarts").GetCollection<ShoppingCart>("carts");
var cartsRepo = new MongoDbRepository<ShoppingCart, Guid>(_client.GetDatabase("amazonCarts"), "carts");
var cart = await cartsRepo.GetInstanceAsync(Guid.Parse("b1c77a6a-cd03-4418-8253-bb1d08429523"));
cart.SetDeliverToAddress(5);
await cartsRepo.CommitAsync();

//var factory = new ShoppingCartFactory();
//var cart = collection.Find(x => x.Id == Guid.Parse("b1c77a6a-cd03-4418-8253-bb1d08429523")).First();
Console.Read();
//await collection.InsertOneAsync(factory.Create(Guid.NewGuid()));


