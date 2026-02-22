using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;

/// <summary>
/// Represents the input for the GetAllVehicles use case.
/// </summary>
public class ReturnVehicleInputDto : IUseCaseInput
{
    /// <summary>
    /// Return date of the vehicle to be rented.
    /// </summary>
    public DateTime ReturnDate { get; set; }

    /// <summary>
    /// Vehicle identifier for the vehicle to be rented.
    /// </summary>
    public Guid VehicleId { get; set; }
}

