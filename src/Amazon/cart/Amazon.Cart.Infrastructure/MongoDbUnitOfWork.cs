using Amazon.Cart.Domain;
using Amazon.Cart.Domain.Payments;
using Amazon.Cart.Domain.Products;
using Amazon.Cart.Infrastructure.Data.Models;
using Amazon.SharedKernel.Data.NoSql;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;
using MongoDB.Bson;

namespace Amazon.Cart.Infrastructure;

public class MongoDbUnitOfWork(
    MongoDbRepository<ShoppingCart, Guid> _carts,
    MongoDbRepository<Product, Guid> _products,
    MongoDbRepository<PaymentMethod, Guid> _paymentMethods,
    MongoDbRepository<OutboxMessage, int> _events,
    MongoDbRepository<CustomerClaim, ObjectId> _customerClaims
    ) : IUnitOfWork
{

    public async Task CommitAsync()
    {
        await _carts.CommitAsync();
        await _products.CommitAsync();
        await _events.CommitAsync();
        await _customerClaims.CommitAsync();
        await _paymentMethods.CommitAsync();
    }
}