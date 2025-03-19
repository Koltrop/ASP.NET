using MongoDB.Driver;
using Pcf.GivingToCustomer.Core.Domain;
using System;
using System.Linq;

namespace Pcf.GivingToCustomer.DataAccess.Data;

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
        ResetCollection(MongoConstants.PreferenceCollectionName);
        ResetCollection(MongoConstants.PromocodeCollectionName);
        ResetCollection(MongoConstants.CustomerCollectionName);
        ResetCollection(MongoConstants.PromocodeCustomerCollectionName);

        AddPreferences();
        AddCustomers();

    }

    private void AddPreferences()
    {
        database.GetCollection<Preference>(MongoConstants.PreferenceCollectionName)
            .InsertMany(FakeDataFactory.Preferences);
    }

    private void AddCustomers()
    {
        var customers = FakeDataFactory.Customers;
        var preferences = FakeDataFactory.Preferences;
        foreach (var customer in customers)
        {
            var preferenceIds = customer.CustomerPreferences.Select(x => x.PreferenceId).ToList();
            customer.Preferences = preferences.Where(x => preferenceIds.Contains(x.Id)).ToList();
        }
        database.GetCollection<Customer>(MongoConstants.CustomerCollectionName)
            .InsertMany(customers);
    }

    private void ResetCollection(string collectionName)
    {
        database.DropCollection(collectionName);
        database.CreateCollection(collectionName);
    }

}