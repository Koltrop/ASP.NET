using MongoDB.Driver;
using Pcf.GivingToCustomer.Core.Domain;

namespace Pcf.GivingToCustomer.DataAccess.Data;

public sealed class PromocodeCustomersRepository(IMongoClient client)
    : MongoRepositoryBase<PromoCodeCustomer>(client, MongoConstants.PromocodeCustomerCollectionName);
