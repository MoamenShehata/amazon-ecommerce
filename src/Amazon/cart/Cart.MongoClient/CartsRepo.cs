using Amazon.Cart.Domain;
using Moamen.SDKs.Repository;
using Moamen.SDKs.Repository.Pagination;
using System.Linq.Expressions;

namespace Cart.MongoClient;

internal class CartsRepo : IRepository<ShoppingCart, Guid>
{
    public void Add(ShoppingCart aggregate)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync(Expression<Func<ShoppingCart, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(Expression<Func<ShoppingCart, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ShoppingCart>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ShoppingCart>> GetAllAsync(Expression<Func<ShoppingCart, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TProjection>> GetAllAsync<TProjection>(Expression<Func<ShoppingCart, TProjection>> projector)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TProjection>> GetAllAsync<TProjection>(Expression<Func<ShoppingCart, bool>> predicate, Expression<Func<ShoppingCart, TProjection>> projector)
    {
        throw new NotImplementedException();
    }

    public Task<ShoppingCart> GetInstanceAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<TProjection> GetInstanceAsync<TProjection>(Guid id, Expression<Func<ShoppingCart, TProjection>> projector)
    {
        throw new NotImplementedException();
    }

    public Task<ShoppingCart> GetInstanceAsync(Expression<Func<ShoppingCart, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<TProjection> GetInstanceAsync<TProjection>(Expression<Func<ShoppingCart, bool>> predicate, Expression<Func<ShoppingCart, TProjection>> projector)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<ShoppingCart, TOrderBy>> GetPageAsync<TOrderBy>(PagedRequest pagedRequest, Expression<Func<ShoppingCart, TOrderBy>> orderByKey)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<ShoppingCart, TOrderBy>> GetPageAsync<TOrderBy>(PagedRequest pagedRequest, Expression<Func<ShoppingCart, TOrderBy>> orderByKey, List<Expression<Func<ShoppingCart, bool>>> filters)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<ShoppingCart, TKeySet>> GetPageAsync<TKeySet>(int pageSize, Expression<Func<ShoppingCart, TKeySet>> keySetSelector, TKeySet lastSeenValue) where TKeySet : IComparable<TKeySet>
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<ShoppingCart, TKeySet>> GetPageAsync<TKeySet>(int pageSize, Expression<Func<ShoppingCart, TKeySet>> keySetSelector, TKeySet lastSeenValue, List<Expression<Func<ShoppingCart, bool>>> filters) where TKeySet : IComparable<TKeySet>
    {
        throw new NotImplementedException();
    }

    public void Remove(ShoppingCart aggregate)
    {
        throw new NotImplementedException();
    }
}
