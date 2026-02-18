using System.Collections.Generic;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;

/// <summary>
/// Represents the output for the GetAllVehicles use case.
/// </summary>
public sealed class GetAllVehiclesOutputDto : IUseCaseOutput
{
    /// <summary>
    /// Gets or sets the collection of vehicles.
    /// </summary>
    public IEnumerable<VehicleDto> Vehicles { get; set; } = new List<VehicleDto>();
}
