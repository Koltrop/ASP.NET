using MongoDB.Driver;
using Pcf.GivingToCustomer.Core.Domain;

namespace Pcf.GivingToCustomer.DataAccess.Data;

public sealed class PreferenceRepository(IMongoClient client)
    : MongoRepositoryBase<Preference>(client, MongoConstants.PreferenceCollectionName);