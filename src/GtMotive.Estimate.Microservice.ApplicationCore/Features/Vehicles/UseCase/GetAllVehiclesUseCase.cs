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

    /// <summary>
    /// Initializes a new instance of the GetAllVehiclesUseCase class, which coordinates the retrieval of all vehicle
    /// records using the specified repository and output port.
    /// </summary>
    /// <param name="vehicleRepository">The repository used to access vehicle data. This parameter must not be null and should implement the
    /// IVehicleRepository interface.</param>
    /// <param name="outputPort">The output port.</param>
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
