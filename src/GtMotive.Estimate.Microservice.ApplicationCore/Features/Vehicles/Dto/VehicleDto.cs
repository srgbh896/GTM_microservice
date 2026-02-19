using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;

/// <summary>
/// Represents a vehicle data transfer object containing essential vehicle information.
/// </summary>
public class VehicleDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the vehicle.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the brand or manufacturer of the vehicle.
    /// </summary>
    public string Brand { get; set; }

    /// <summary>
    /// Gets or sets the model name of the vehicle.
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Gets or sets the license plate number of the vehicle.
    /// </summary>
    public string LicensePlate { get; set; }

    /// <summary>
    /// Gets or sets the manufacturing date of the vehicle.
    /// </summary>
    public DateTime ManufacturingDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the vehicle is currently rented.
    /// </summary>
    public bool IsRented { get; set; }
}
