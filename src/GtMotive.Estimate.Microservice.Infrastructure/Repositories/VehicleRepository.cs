using GtMotive.Estimate.Microservice.ApplicationCore.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Infrastructure.Base;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;

namespace GtMotive.Estimate.Microservice.Infrastructure.Repositories;

/// <summary>
/// Represents a repository that provides data access operations for vehicle entities using a MongoDB database.
/// </summary>
/// <remarks>This repository inherits from RepositoryBase and implements IVehicleRepository, offering CRUD
/// functionality for vehicles. It is intended to be used as part of the data access layer in applications that require
/// persistent storage and retrieval of vehicle information.</remarks>
/// <param name="service">The MongoService instance used to interact with the MongoDB database for vehicle data operations.</param>
public class VehicleRepository(MongoService service) : RepositoryBase<Vehicle>(service, "vehicles"), IVehicleRepository
{
}
