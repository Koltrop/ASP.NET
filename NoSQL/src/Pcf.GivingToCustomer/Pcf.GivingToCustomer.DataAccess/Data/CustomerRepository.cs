using MongoDB.Driver;
using Pcf.GivingToCustomer.Core.Domain;

namespace Pcf.GivingToCustomer.DataAccess.Data;

public sealed class CustomerRepository(IMongoClient client)
    : MongoRepositoryBase<Customer>(client, MongoConstants.CustomerCollectionName);