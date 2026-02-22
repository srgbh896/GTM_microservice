using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto.Base;
using GtMotive.Estimate.Microservice.ApplicationCore.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.UseCase;

/// <summary>
/// Use case for creating a new vehicle in the fleet.
/// </summary>
public sealed class ReturnVehicleUseCase : IUseCase<ReturnVehicleInputDto>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IRentRepository _rentRepository;
    private readonly IOutputPortStandard<Result<ReturnVehicleOutputDto>> _outputPort;

    /// <summary>
    /// Initializes a new instance of the CreateVehicleUseCase class.
    /// </summary>
    /// <param name="rentRepository">Repository for rents persistence.</param>
    /// <param name="vehicleRepository">Repository for vehicle persistence.</param>
    /// <param name="outputPort">Output port to return the result.</param>
    public ReturnVehicleUseCase(
        IRentRepository rentRepository,
        IVehicleRepository vehicleRepository,
        IOutputPortStandard<Result<ReturnVehicleOutputDto>> outputPort)
    {
        _rentRepository = rentRepository;
        _vehicleRepository = vehicleRepository;
        _outputPort = outputPort;
    }

    /// <summary>
    /// Executes the use case to create a new vehicle.
    /// </summary>
    /// <param name="input">The input DTO containing vehicle data.</param>
    public async Task Execute(ReturnVehicleInputDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        // 1. Get vehicle
        var vehicle = await _vehicleRepository.GetByIdAsync(input.VehicleId);
        if (vehicle is null)
        {
            _outputPort.StandardHandle(Result.Failure<ReturnVehicleOutputDto>("Vehicle not found."));
            return;
        }

        // 2. Get last active rent (no finishing date)
        var rent = await _rentRepository.GetLastActiveByVehicleIdAsync(input.VehicleId);
        if (rent is null)
        {
            _outputPort.StandardHandle(Result.Failure<ReturnVehicleOutputDto>("No active rent found for this vehicle."));
            return;
        }

        // 3. Finish rent
        rent.ReturnDate = input.ReturnDate;

        // 4. Mark vehicle as available
        vehicle.IsRented = false;

        // 5. Persist changes
        await _rentRepository.ReplaceOneAsync(rent);
        await _vehicleRepository.ReplaceOneAsync(vehicle);

        // 6. Map result (if needed)
        var output = new ReturnVehicleOutputDto
        {
            Id = rent.Id
        };

        // Send result to output port (presenter)
        _outputPort.StandardHandle(Result<ReturnVehicleOutputDto>.Success(output));
    }
}
