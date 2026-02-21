using System;
using System.ComponentModel.DataAnnotations;
using GtMotive.Estimate.Microservice.Api.UseCases;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.Features.Vehicles.GetAllVehicles;

/// <summary>
/// Represents a request to retrieve all vehicles.
/// This is the MediatR request object that will be dispatched through the mediator.
/// </summary>
public sealed class CreateVehicleCommand : IRequest<IWebApiPresenter>
{
    /// <summary>
    /// Brand of the vehicle to be created.
    /// </summary>
    [Required]
    public string Brand { get; set; }

    /// <summary>
    /// model of the vehicle to be created.
    /// </summary>
    [Required]
    public string Model { get; set; }

    /// <summary>
    /// License plate of the vehicle to be created.
    /// </summary>
    [Required]
    public string LicensePlate { get; set; }

    /// <summary>
    /// Manufacturing date of the vehicle to be created.
    /// </summary>
    [Required]
    public DateTime? ManufacturingDate { get; set; }
}
