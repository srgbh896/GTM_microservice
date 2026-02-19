using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Migration;

/// <summary>
/// Initializes the MongoDB database with required indexes and seed data.
/// </summary>
public class MongoInit(MongoService mongoService)
{
    private readonly IMongoDatabase _database = mongoService.Database;

    /// <summary>
    /// Asynchronously initializes the MongoDB database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InitializeAsync()
    {
        await CreateVehicleIndexes();
    }

    /// <summary>
    /// Creates indexes for the vehicles collection and seeds initial data.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task CreateVehicleIndexes()
    {
        var collection = _database.GetCollection<Vehicle>("vehicles");

        var licensePlateIndex = new CreateIndexModel<Vehicle>(
            Builders<Vehicle>.IndexKeys.Ascending(v => v.LicensePlate),
            new CreateIndexOptions { Unique = true });

        await collection.Indexes.CreateOneAsync(licensePlateIndex);
        await SeedVehicles();
    }

    /// <summary>
    /// Seeds the vehicles collection with initial data if it is empty.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
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
