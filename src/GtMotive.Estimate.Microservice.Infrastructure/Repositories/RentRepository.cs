using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Infrastructure.Base;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.Repositories;

/// <summary>
/// Represents a repository that provides data access operations for vehicle entities using a MongoDB database.
/// </summary>
/// <remarks>This repository inherits from RepositoryBase and implements IRentRepository, offering CRUD
/// functionality for vehicles. It is intended to be used as part of the data access layer in applications that require
/// persistent storage and retrieval of vehicle information.</remarks>
/// <param name="service">The MongoService instance used to interact with the MongoDB database for vehicle data operations.</param>
public class RentRepository(MongoService service) : RepositoryBase<Rent>(service, "rents"), IRentRepository
{
    /// <summary>
    /// Gets the last active rent for a given vehicle ID.
    /// </summary>
    /// <param name="vehicleId"></param>
    /// <returns>Rent</returns>
    public async Task<Rent> GetLastActiveByVehicleIdAsync(Guid vehicleId)
    {
        var filter = Builders<Rent>.Filter.And(
            Builders<Rent>.Filter.Eq(r => r.VehicleId, vehicleId),
            Builders<Rent>.Filter.Eq(r => r.ReturnDate, null)
        );

        var sort = Builders<Rent>.Sort.Descending(r => r.StartDate);

        return await _collection
            .Find(filter)
            .Sort(sort)
            .FirstOrDefaultAsync();
    }
}
