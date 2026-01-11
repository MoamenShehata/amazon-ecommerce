using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moamen.SDKs.Repository;
using Moamen.SDKs.Repository.Pagination;

namespace Amazon.Orders.Domain.Tests
{
    public class InMemoryRepo<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : class, IEntity<TId> where TId : IEquatable<TId>
    {
        private InMemoryContext _ctxt = new InMemoryContext();

        public void Add(TEntity aggregate)
        {
            _ctxt.Set<TEntity>().Add(aggregate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _ctxt.Set<TEntity>().CountAsync(predicate);
        }

        public Task<bool> ExistsAsync(TId id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await GetAllAsync(x => true);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAllAsync(predicate, x => x);
        }

        public async Task<IEnumerable<TProjection>> GetAllAsync<TProjection>(Expression<Func<TEntity, TProjection>> projector)
        {
            return await GetAllAsync(x => true, projector);
        }

        public async Task<IEnumerable<TProjection>> GetAllAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjection>> projector)
        {
            return await _ctxt.Set<TEntity>().Where(predicate).Select(projector).ToListAsync();
        }

        public Task<TEntity> GetInstanceAsync(TId id)
        {
            throw new NotImplementedException();
        }

        public Task<TProjection> GetInstanceAsync<TProjection>(TId id, Expression<Func<TEntity, TProjection>> projector)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetInstanceAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<TProjection> GetInstanceAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjection>> projector)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<TEntity, TOrderBy>> GetPageAsync<TOrderBy>(PagedRequest pagedRequest, Expression<Func<TEntity, TOrderBy>> orderByKey)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<TEntity, TOrderBy>> GetPageAsync<TOrderBy>(PagedRequest pagedRequest, Expression<Func<TEntity, TOrderBy>> orderByKey, List<Expression<Func<TEntity, bool>>> filters)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<TEntity, TKeySet>> GetPageAsync<TKeySet>(int pageSize, Expression<Func<TEntity, TKeySet>> keySetSelector, TKeySet lastSeenValue) where TKeySet : IComparable<TKeySet>
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<TEntity, TKeySet>> GetPageAsync<TKeySet>(int pageSize, Expression<Func<TEntity, TKeySet>> keySetSelector, TKeySet lastSeenValue, List<Expression<Func<TEntity, bool>>> filters) where TKeySet : IComparable<TKeySet>
        {
            throw new NotImplementedException();
        }

        public void Remove(TEntity aggregate)
        {
            throw new NotImplementedException();
        }
    }

    public static class RepoFactory
    {
        public static IRepository<TEntity, TId> Create<TEntity, TId>() where TEntity : class, IEntity<TId> where TId : IEquatable<TId> => new InMemoryRepo<TEntity, TId>();
    }
}