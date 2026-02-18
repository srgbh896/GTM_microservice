using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;
using GtMotive.Estimate.Microservice.ApplicationCore.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.UseCase;

/// <summary>
/// Use case for retrieving all vehicles.
/// </summary>
public sealed class GetAllVehiclesUseCase : IUseCase<GetAllVehiclesInputDto>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IOutputPortStandard<GetAllVehiclesOutputDto> _outputPort;

    public GetAllVehiclesUseCase(
        IVehicleRepository vehicleRepository,
        IOutputPortStandard<GetAllVehiclesOutputDto> outputPort)
    {
        _vehicleRepository = vehicleRepository;
        _outputPort = outputPort;
    }

    /// <summary>
    /// Executes the GetAllVehicles use case.
    /// </summary>
    /// <param name="input">The input for the use case.</param>
    public async Task Execute(GetAllVehiclesInputDto input)
    {
        // Retrieve all vehicles from the repository
        var vehicles = await _vehicleRepository.GetAllAsync();

        // Map domain entities to DTOs
        var vehicleDtos = vehicles
            .Select(v => new VehicleDto
            {
                Id = v.Id,
                Brand = v.Brand,
                Model = v.Model,
                LicensePlate = v.LicensePlate,
                ManufacturingDate = v.ManufacturingDate,
                IsRented = v.IsRented
            })
            .ToList();

        // Create output with the vehicles collection
        var output = new GetAllVehiclesOutputDto
        {
            Vehicles = vehicleDtos
        };

        // Write to output port (presenter will format the response)
        _outputPort.StandardHandle(output);
    }
}
