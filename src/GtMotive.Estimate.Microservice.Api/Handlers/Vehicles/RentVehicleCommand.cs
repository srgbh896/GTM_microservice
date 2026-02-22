using System;
using System.ComponentModel.DataAnnotations;
using GtMotive.Estimate.Microservice.Api.UseCases;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GtMotive.Estimate.Microservice.Api.Features.Vehicles.GetAllVehicles;

/// <summary>
/// RentVehicleCommand represents a request to rent a vehicle.
/// </summary>
public sealed class RentVehicleCommand : IRequest<IWebApiPresenter>
{
    /// <summary>
    /// Start date of the vehicle to be rented.
    /// </summary>
    [Required]
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Return date of the vehicle to be rented.
    /// </summary>
    [Required]
    public DateTime? PlannedReturnDate { get; set; }

    /// <summary>
    /// Customer identifier for the vehicle to be rented.
    /// </summary>
    [Required]
    public string CustomerIdentifier { get; set; }

    /// <summary>
    /// Vehicle identifier for the vehicle to be rented.
    /// </summary>
    [BindNever]
    public Guid VehicleId { get; set; }
}
