using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;

/// <summary>
/// Represents the input for the GetAllVehicles use case.
/// </summary>
public class ReturnVehicleOutputDto : IUseCaseOutput
{
    /// <summary>
    /// id of the created vehicle
    /// </summary>
    public Guid Id { get; set; }
}

