using System;
using System.Threading.Tasks;
using AutoMapper;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto.Base;
using GtMotive.Estimate.Microservice.ApplicationCore.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Entities.ValueObj;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.UseCase;

/// <summary>
/// Use case for creating a new vehicle in the fleet.
/// </summary>
public sealed class CreateVehicleUseCase : IUseCase<CreateVehicleInputDto>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IOutputPortStandard<Result<CreateVehicleOutputDto>> _outputPort;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the CreateVehicleUseCase class.
    /// </summary>
    /// <param name="vehicleRepository">Repository for vehicle persistence.</param>
    /// <param name="outputPort">Output port to return the result.</param>
    /// <param name="mapper">AutoMapper instance to map domain entities to DTOs.</param>
    public CreateVehicleUseCase(
        IVehicleRepository vehicleRepository,
        IOutputPortStandard<Result<CreateVehicleOutputDto>> outputPort,
        IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _outputPort = outputPort;
        _mapper = mapper;
    }

    /// <summary>
    /// Executes the use case to create a new vehicle.
    /// </summary>
    /// <param name="input">The input DTO containing vehicle data.</param>
    public async Task Execute(CreateVehicleInputDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var licensePlate = Plate.Create(input.LicensePlate);
        var vehicle = _mapper.Map<Vehicle>(input);

        // Persist vehicle to repository
        await _vehicleRepository.InsertOneAsync(vehicle);

        // Map the newly created vehicle to output DTO
        var output = new CreateVehicleOutputDto
        {
            Id = vehicle.Id
        };

        // Send result to output port (presenter)
        _outputPort.StandardHandle(Result<CreateVehicleOutputDto>.Success(output));
    }
}
