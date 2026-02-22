using System;
using System.ComponentModel.DataAnnotations;
using GtMotive.Estimate.Microservice.Api.UseCases;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.Features.Vehicles.GetAllVehicles;

/// <summary>
/// Represents a request to retrieve all vehicles.
/// This is the MediatR request object that will be dispatched through the mediator.
/// </summary>
public sealed class ReturnVehicleCommand : IRequest<IWebApiPresenter>
{
    /// <summary>
    /// Vehicle identifier for the vehicle to be returned.
    /// </summary>
    [Required]
    public Guid VehicleId { get; set; }

    /// <summary>
    /// Return date
    /// </summary>
    [Required]
    public DateTime ReturnDate { get; set; }
}
