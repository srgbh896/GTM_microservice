using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb;

public class MongoService
{
    private readonly MongoDbSettings _settings;

    public MongoClient MongoClient { get; }
    public IMongoDatabase Database { get; }

    public MongoService(IOptions<MongoDbSettings> options)
    {
        _settings = options.Value;

        MongoClient = new MongoClient(_settings.ConnectionString);
        Database = MongoClient.GetDatabase(_settings.MongoDbDatabaseName);
    }
}
