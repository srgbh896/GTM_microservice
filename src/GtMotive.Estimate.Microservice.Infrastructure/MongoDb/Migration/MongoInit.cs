using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Migration;

public class MongoInit(MongoService mongoService)
{
    private readonly IMongoDatabase _database = mongoService.Database;

    public async Task InitializeAsync()
    {
        await CreateVehicleIndexes();
    }

    private async Task CreateVehicleIndexes()
    {
        var collection = _database.GetCollection<Vehicle>("vehicles");

        var licensePlateIndex = new CreateIndexModel<Vehicle>(
            Builders<Vehicle>.IndexKeys.Ascending(v => v.LicensePlate),
            new CreateIndexOptions { Unique = true });

        await collection.Indexes.CreateOneAsync(licensePlateIndex);
        await SeedVehicles();
    }

    private async Task SeedVehicles()
    {
        var collection = _database.GetCollection<Vehicle>("vehicles");

        var count = await collection.CountDocumentsAsync(_ => true);

        if (count == 0)
        {
            await collection.InsertManyAsync(
            [
            new Vehicle(){ IsRented = false, LicensePlate = "Test", Brand = "Toyota", Model = "Test" }
        ]);
        }
    }
}
