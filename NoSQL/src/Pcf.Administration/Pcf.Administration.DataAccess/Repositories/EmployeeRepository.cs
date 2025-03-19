using MongoDB.Driver;
using Pcf.Administration.Core.Domain.Administration;

namespace Pcf.Administration.DataAccess.Repositories;

public sealed class EmployeeRepository(IMongoClient client)
    : MongoRepositoryBase<Employee>(client, MongoConstants.EmployeeCollectionName);
