using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;

/// <summary>
/// Represents the input for the GetAllVehicles use case.
/// </summary>
public class RentVehicleInputDto : IUseCaseInput
{
    /// <summary>
    /// Start date of the vehicle to be rented.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Return date of the vehicle to be rented.
    /// </summary>
    public DateTime PlannedReturnDate { get; set; }

    /// <summary>
    /// Customer identifier for the vehicle to be rented.
    /// </summary>
    public string CustomerIdentifier { get; set; }

    /// <summary>
    /// Vehicle identifier for the vehicle to be rented.
    /// </summary>
    public Guid VehicleId { get; set; }
}

