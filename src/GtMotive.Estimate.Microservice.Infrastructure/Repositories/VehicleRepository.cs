using GtMotive.Estimate.Microservice.ApplicationCore.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Infrastructure.Base;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;

namespace GtMotive.Estimate.Microservice.Infrastructure.Repositories;

public class VehicleRepository(MongoService service) : RepositoryBase<Vehicle>(service, "vehicles"), IVehicleRepository
{
}
