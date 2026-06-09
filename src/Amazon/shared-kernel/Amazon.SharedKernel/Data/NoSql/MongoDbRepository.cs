using Moamen.SDKs.Repository;
using Moamen.SDKs.Repository.Pagination;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Definitions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Amazon.SharedKernel.Data.NoSql;

public class MongoDbRepository<TCollection, TKey>(IMongoDatabase _database, string collectionName)
    : IRepository<TCollection, TKey> where TCollection : class, IEntity<TKey> where TKey : IEquatable<TKey>
{
    private readonly IMongoCollection<TCollection> _collection = _database.GetCollection<TCollection>(collectionName);

    private readonly Dictionary<TKey, TCollection> _trackedInstances = new();

    public void Add(TCollection aggregate)
    {
        _collection.InsertOne(aggregate);
        //TrackInstance(aggregate);
    }

    public async Task<IEnumerable<TProjection>> GetAllAsync<TProjection>(Expression<Func<TCollection, bool>> predicate, Expression<Func<TCollection, TProjection>> projector)
    {
        var collection = (await _collection.FindAsync(predicate)).ToList();
        foreach (var item in collection)
            TrackInstance(item);

        return collection.Select(x => projector.Compile()(x));
    }

    public async Task<IEnumerable<TProjection>> GetAllAsync<TProjection>(Expression<Func<TCollection, TProjection>> projector) => await GetAllAsync(x => true, projector);
    public async Task<IEnumerable<TCollection>> GetAllAsync(Expression<Func<TCollection, bool>> predicate) => await GetAllAsync(predicate, x => x);
    public async Task<IEnumerable<TCollection>> GetAllAsync() => await GetAllAsync(x => true);

    public async Task<int> CountAsync(Expression<Func<TCollection, bool>> predicate) => (int)await _collection.CountDocumentsAsync(predicate);

    public async Task<bool> ExistsAsync(Expression<Func<TCollection, bool>> predicate) => (await CountAsync(predicate)) > 0;
    public async Task<bool> ExistsAsync(TKey id) => await ExistsAsync(x => x.Id.Equals(id));

    public async Task<TProjection> GetInstanceAsync<TProjection>(Expression<Func<TCollection, bool>> predicate, Expression<Func<TCollection, TProjection>> projector)
    {
        var instance = (await _collection.FindAsync(predicate)).FirstOrDefault();
        if (instance == null) return default;

        TrackInstance(instance);

        return projector.Compile()(instance);
    }
    public async Task<TCollection> GetInstanceAsync(Expression<Func<TCollection, bool>> predicate) => await GetInstanceAsync(predicate, x => x);
    public async Task<TProjection> GetInstanceAsync<TProjection>(TKey id, Expression<Func<TCollection, TProjection>> projector) => await GetInstanceAsync(x => x.Id.Equals(id), projector);
    public async Task<TCollection> GetInstanceAsync(TKey id) => await GetInstanceAsync(id, x => x);


    public async Task<PagedResult<TCollection, TOrderBy>> GetPageAsync<TOrderBy>(PagedRequest pagedRequest, Expression<Func<TCollection, TOrderBy>> orderByKey) => await GetPageAsync(pagedRequest, orderByKey, [x => true]);

    public async Task<PagedResult<TCollection, TOrderBy>> GetPageAsync<TOrderBy>(PagedRequest pagedRequest, Expression<Func<TCollection, TOrderBy>> orderByKey, List<Expression<Func<TCollection, bool>>> filters)
    {
        var queryBase = _collection.AsQueryable();

        foreach (var filter in filters)
            queryBase = queryBase.Where(filter);

        var entities = await queryBase
            .OrderBy(orderByKey)
            .Skip((pagedRequest.PageNumber - 1) * pagedRequest.PageSize)
            .Take(pagedRequest.PageSize)
            .ToListAsync();

        var totalCount = await queryBase.CountAsync();

        var lastSeenRecord = entities.LastOrDefault()!;
        var lastSeen = lastSeenRecord != null ? orderByKey.Compile()(lastSeenRecord) : default;

        return new PagedResult<TCollection, TOrderBy>(entities, totalCount, lastSeen);
    }

    public async Task<PagedResult<TCollection, TKeySet>> GetPageAsync<TKeySet>(int pageSize, Expression<Func<TCollection, TKeySet>> keySetSelector, TKeySet lastSeenValue) where TKeySet : IComparable<TKeySet> => await GetPageAsync(pageSize, keySetSelector, lastSeenValue, [x => true]);

    public async Task<PagedResult<TCollection, TKeySet>> GetPageAsync<TKeySet>(int pageSize, Expression<Func<TCollection, TKeySet>> keySetSelector, TKeySet lastSeenValue, List<Expression<Func<TCollection, bool>>> filters)
        where TKeySet : IComparable<TKeySet>
    {
        pageSize = Math.Clamp(pageSize, 10, 100);

        var parameter = keySetSelector.Parameters[0];
        var body = Expression.GreaterThan(
            keySetSelector.Body,
            Expression.Constant(lastSeenValue)
        );

        var keySelectorPredicate = Expression.Lambda<Func<TCollection, bool>>(body, parameter);

        IQueryable<TCollection> queryBase = _collection.AsQueryable();
        foreach (var filter in filters)
            queryBase = queryBase.Where(filter);

        var entities = await queryBase
            .Where(keySelectorPredicate)
            .OrderBy(keySetSelector)
            .Take(pageSize)
            .ToListAsync();

        var totalCount = await queryBase.CountAsync();

        var lastSeenRecord = entities.LastOrDefault()!;
        var lastSeen = lastSeenRecord != null ? keySetSelector.Compile()(lastSeenRecord) : default;

        return new PagedResult<TCollection, TKeySet>(entities, totalCount, lastSeen);
    }


    public void Remove(TCollection aggregate) => _collection.DeleteOne(x => x.Id.Equals(aggregate.Id));

    private void TrackInstance(TCollection instance)
    {
        if (!_trackedInstances.ContainsKey(instance.Id))
            _trackedInstances.Add(instance.Id, instance);
    }

    public async Task CommitAsync()
    {
        foreach (var instanceToUpdate in _trackedInstances)
            await _collection.ReplaceOneAsync(x => x.Id.Equals(instanceToUpdate.Key), instanceToUpdate.Value);
    }
}