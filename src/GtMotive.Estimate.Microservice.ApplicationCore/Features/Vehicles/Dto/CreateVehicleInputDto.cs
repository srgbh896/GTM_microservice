using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;

/// <summary>
/// Represents the input for the GetAllVehicles use case.
/// </summary>
public class CreateVehicleInputDto : IUseCaseInput
{
    /// <summary>
    /// Brand of the vehicle.
    /// </summary>
    public string Brand { get; set; }

    /// <summary>
    /// Model of the vehicle.
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// License plate of the vehicle.
    /// </summary>
    public string LicensePlate { get; set; }

    /// <summary>
    /// Manufacturing date of the vehicle.
    /// </summary>
    public DateTime ManufacturingDate { get; set; }
}

