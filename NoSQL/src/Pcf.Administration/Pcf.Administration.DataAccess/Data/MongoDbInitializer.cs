using MongoDB.Driver;
using Pcf.Administration.Core.Domain.Administration;

namespace Pcf.Administration.DataAccess.Data;

public class MongoDbInitializer
    : IDbInitializer
{
    private readonly IMongoDatabase database;

    public MongoDbInitializer(IMongoClient client)
    {
        database = client.GetDatabase(MongoConstants.DatabaseName);
    }

    public void InitializeDb()
    {
        ResetCollection(MongoConstants.EmployeeCollectionName);
        ResetCollection(MongoConstants.RoleCollectionName);

        AddRoles();
        AddEmployees();
    }

    private void AddRoles()
    {
        database.GetCollection<Role>(MongoConstants.RoleCollectionName)
            .InsertMany(FakeDataFactory.Roles);
    }

    private void AddEmployees()
    {
        database.GetCollection<Employee>(MongoConstants.EmployeeCollectionName)
            .InsertMany(FakeDataFactory.Employees);
    }

    private void ResetCollection(string collectionName)
    {
        database.DropCollection(collectionName);
        database.CreateCollection(collectionName);
    }

}