using MongoDB.Driver;
using Pcf.GivingToCustomer.Core.Domain;

namespace Pcf.GivingToCustomer.DataAccess.Data;

public sealed class PromocodeRepository(IMongoClient client)
    : MongoRepositoryBase<PromoCode>(client, MongoConstants.PromocodeCollectionName);
