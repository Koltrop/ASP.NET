using MongoDB.Driver;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.DataAccess.Data;

public abstract class MongoRepositoryBase<T> : IRepository<T> where T : BaseEntity
{
    protected IMongoDatabase Database { get; }

    protected IMongoCollection<T> Collection { get; }

    public MongoRepositoryBase(IMongoClient client, string collectionName)
    {
        Database = client.GetDatabase(MongoConstants.DatabaseName);
        Collection = Database.GetCollection<T>(collectionName);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Collection.Find(_ => true).ToListAsync();
    }

    public virtual async Task<T> GetByIdAsync(Guid id)
    {
        return await Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
    {
        return await Collection.Find(x => ids.Contains(x.Id)).ToListAsync();
    }

    public virtual async Task<T> GetFirstWhere(Expression<Func<T, bool>> predicate)
    {
        return await Collection.Find(predicate).FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
    {
        return await Collection.Find(predicate).ToListAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        await Collection.InsertOneAsync(entity);
    }

    public virtual async Task UpdateAsync(T entity)
    {
        await Collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
    }

    public virtual async Task DeleteAsync(T entity)
    {
        await Collection.DeleteOneAsync(x => x.Id == entity.Id);
    }
}
