using MongoDB.Driver;
using Pcf.Administration.Core.Domain.Administration;
using System.Threading.Tasks;

namespace Pcf.Administration.DataAccess.Repositories;

public sealed class RoleRepository(IMongoClient client)
    : MongoRepositoryBase<Role>(client, MongoConstants.RoleCollectionName)
{
    public override async Task UpdateAsync(Role entity)
    {       
        await base.UpdateAsync(entity);

        // Update all related entries in Employee collection
        var filter = Builders<Employee>.Filter.Eq(x => x.Role.Id, entity.Id);
        var changes = Builders<Employee>.Update.Set(x => x.Role, entity);
        await Database.GetCollection<Employee>(MongoConstants.EmployeeCollectionName)
                .UpdateManyAsync(filter, changes);
    }
}
