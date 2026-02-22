using System;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto.Base;
using GtMotive.Estimate.Microservice.ApplicationCore.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.UseCase;

/// <summary>
/// Use case for creating a new vehicle in the fleet.
/// </summary>
public sealed class RentVehicleUseCase : IUseCase<RentVehicleInputDto>
{
    private readonly IRentRepository _rentRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IOutputPortStandard<Result<RentVehicleOutputDto>> _outputPort;

    /// <summary>
    /// Initializes a new instance of the CreateVehicleUseCase class.
    /// </summary>
    /// <param name="vehicleRepository">Repository for vehicle persistence.</param>
    /// <param name="rentRepository">Repository for rents persistence.</param>
    /// <param name="outputPort">Output port to return the result.</param>
    public RentVehicleUseCase(
        IRentRepository rentRepository,
        IVehicleRepository vehicleRepository,
        IOutputPortStandard<Result<RentVehicleOutputDto>> outputPort)
    {
        _rentRepository = rentRepository;
        _vehicleRepository = vehicleRepository;
        _outputPort = outputPort;
    }

    /// <summary>
    /// Executes the use case to create a new vehicle.
    /// </summary>
    /// <param name="input">The input DTO containing vehicle data.</param>
    public async Task Execute(RentVehicleInputDto input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var vehicle = await _vehicleRepository.GetByIdAsync(input.VehicleId);
        if (vehicle is null)
        {
            _outputPort.StandardHandle(Result.Failure<RentVehicleOutputDto>("Vehicle not found."));
            return;
        }

        if (vehicle.IsRented)
        {
            _outputPort.StandardHandle(Result.Failure<RentVehicleOutputDto>("Vehicle is not available for rent."));
            return;
        }

        var activeRent = (await _rentRepository.FilterByAsync(
            r => r.CustomerIdentifier == input.CustomerIdentifier
                 && r.ReturnDate == null))
            .FirstOrDefault();

        if (activeRent is not null)
        {
            _outputPort.StandardHandle(Result.Failure<RentVehicleOutputDto>("Customer already has an active vehicle rent."));
            return;
        }

        var rent = new Rent
        {
            StartDate = input.StartDate!.Value,
            PlannedReturnDate = input.PlannedReturnDate,
            CustomerIdentifier = input.CustomerIdentifier,
            VehicleId = input.VehicleId
        };

        // Persist rent
        await _rentRepository.InsertOneAsync(rent);

        vehicle.IsRented = true;
        await _vehicleRepository.ReplaceOneAsync(vehicle);

        // Map to output DTO
        var output = new RentVehicleOutputDto { Id = rent.Id };
        _outputPort.StandardHandle(Result<RentVehicleOutputDto>.Success(output));
    }
}
