using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Interfaces.Base;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages vehicle entities.
/// </summary>
/// <remarks>This interface extends the base repository interface, providing methods for data access and
/// manipulation specific to vehicle entities. Implementations of this interface should ensure thread safety and handle
/// any data persistence concerns appropriately.</remarks>
public interface IRentRepository : IBaseRepository<Rent>
{
    /// <summary>
    /// Gets the last active rent for a given vehicle ID.
    /// </summary>
    /// <param name="vehicleId"></param>
    /// <returns>Rent</returns>
    Task<Rent> GetLastActiveByVehicleIdAsync(Guid vehicleId);
}
