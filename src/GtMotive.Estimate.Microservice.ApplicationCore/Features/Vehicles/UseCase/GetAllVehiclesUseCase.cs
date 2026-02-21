using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the GetAllVehiclesUseCase class, which coordinates the retrieval of all vehicle
    /// records using the specified repository and output port.
    /// </summary>
    /// <param name="vehicleRepository">The repository used to access vehicle data. This parameter must not be null and should implement the
    /// IVehicleRepository interface.</param>
    /// <param name="outputPort">The output port.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public GetAllVehiclesUseCase(
        IVehicleRepository vehicleRepository,
        IOutputPortStandard<GetAllVehiclesOutputDto> outputPort,
        IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _outputPort = outputPort;
        _mapper = mapper;
    }

    /// <summary>
    /// Executes the GetAllVehicles use case.
    /// </summary>
    /// <param name="input">The input for the use case.</param>
    public async Task Execute(GetAllVehiclesInputDto input)
    {
        // Retrieve all vehicles from the repository
        var vehicles = await _vehicleRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<VehicleDto>>(vehicles);

        // Create output with the vehicles collection
        var output = new GetAllVehiclesOutputDto
        {
            Vehicles = result
        };

        // Write to output port (presenter will format the response)
        _outputPort.StandardHandle(output);
    }
}
