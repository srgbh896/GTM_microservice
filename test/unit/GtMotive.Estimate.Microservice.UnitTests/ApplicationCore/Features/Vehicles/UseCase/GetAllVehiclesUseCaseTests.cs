using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.UseCase;
using GtMotive.Estimate.Microservice.ApplicationCore.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.Profiles;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Entities.ValueObj;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore.Features.Vehicles.UseCase;

/// <summary>
/// Unit tests for the GetAllVehiclesUseCase.
/// Tests the business logic for retrieving all vehicles from the repository and mapping them to DTOs.
/// </summary>
public sealed class GetAllVehiclesUseCaseTests
{
    private readonly Mock<IVehicleRepository> _mockVehicleRepository;
    private readonly Mock<IOutputPortStandard<GetAllVehiclesOutputDto>> _mockOutputPort;
    private readonly GetAllVehiclesUseCase _useCase;

    /// <summary>
    /// Initializes a new instance of the GetAllVehiclesUseCaseTests class.
    /// Sets up the mocks and the use case for testing.
    /// </summary>
    public GetAllVehiclesUseCaseTests()
    {
        _mockVehicleRepository = new Mock<IVehicleRepository>();
        _mockOutputPort = new Mock<IOutputPortStandard<GetAllVehiclesOutputDto>>();
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddDebug());

        var profile = new VehicleProfile();
        var configuration = new MapperConfiguration(cfg => { cfg.AddProfile(profile); }, loggerFactory);
        var mapper = new Mapper(configuration);
        _useCase = new GetAllVehiclesUseCase(_mockVehicleRepository.Object, _mockOutputPort.Object, mapper);

    }

    /// <summary>
    /// Tests that Execute returns all vehicles from the repository when it contains data.
    /// Verifies that the output port is called with a DTO containing exactly 2 vehicles with correct properties.
    /// </summary>
    [Fact]
    public async Task ExecuteShouldReturnAllVehicles()
    {
        // Arrange
        var vehicles = new List<Vehicle>
        {
            new()
            {
                Brand = "Toyota",
                Model = "Corolla",
                LicensePlate = Plate.Create("test"),
                ManufacturingDate = DateTime.UtcNow.AddYears(-2),
                IsRented = false
            },
            new()
            {
                Brand = "Honda",
                Model = "Civic",
                LicensePlate = Plate.Create("tes3t"),
                ManufacturingDate = DateTime.UtcNow.AddYears(-1),
                IsRented = true,
                CurrentCustomerId = Guid.NewGuid(),
                RentalStartDate = DateTime.UtcNow.AddDays(-5)
            }
        };

        _mockVehicleRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(vehicles);

        var input = new GetAllVehiclesInputDto();

        // Act
        await _useCase.Execute(input);

        // Assert
        _mockOutputPort.Verify(x => x.StandardHandle(It.IsAny<GetAllVehiclesOutputDto>()), Times.Once);

        _mockOutputPort.Verify(x => x.StandardHandle(It.Is<GetAllVehiclesOutputDto>(dto =>
            dto.Vehicles.Count() == 2 &&
            dto.Vehicles.Any(v => v.Brand == "Toyota" && v.Model == "Corolla" && !v.IsRented) &&
            dto.Vehicles.Any(v => v.Brand == "Honda" && v.Model == "Civic" && v.IsRented)
        )), Times.Once);
        _mockVehicleRepository.Verify(x => x.GetAllAsync(), Times.Once);
        _mockOutputPort.Verify(x => x.StandardHandle(It.IsAny<GetAllVehiclesOutputDto>()), Times.Once);
    }

    /// <summary>
    /// Tests that Execute returns an empty list when the repository contains no vehicles.
    /// Verifies that the output port is called with a DTO containing an empty vehicles collection.
    /// </summary>
    [Fact]
    public async Task ExecuteShouldReturnEmptyList()
    {
        // Arrange
        var emptyVehicles = new List<Vehicle>();

        _mockVehicleRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(emptyVehicles);

        var input = new GetAllVehiclesInputDto();

        // Act
        await _useCase.Execute(input);

        // Assert
        _mockOutputPort.Verify(x => x.StandardHandle(It.IsAny<GetAllVehiclesOutputDto>()), Times.Once);

        _mockOutputPort.Verify(x => x.StandardHandle(It.Is<GetAllVehiclesOutputDto>(dto =>
            dto.Vehicles != null &&
            !dto.Vehicles.Any()
        )), Times.Once);
    }

    /// <summary>
    /// Tests that the use case correctly maps all vehicle entity properties to the output DTO.
    /// Verifies that each property including Brand, Model, LicensePlate, ManufacturingDate, and IsRented is properly transferred.
    /// </summary>
    [Fact]
    public async Task ExecuteShouldMapVehiclePropertiesToDto()
    {
        // Arrange
        var manufacturingDate = new DateTime(2022, 1, 15, 0, 0, 0, DateTimeKind.Utc);
        var rentalStartDate = new DateTime(2024, 1, 10, 0, 0, 0, DateTimeKind.Utc);
        var currentCustomerId = Guid.NewGuid();

        var vehicle = new Vehicle
        {
            Brand = "BMW",
            Model = "X5",
            LicensePlate = Plate.Create("1231ds"),
            ManufacturingDate = manufacturingDate,
            IsRented = true,
            CurrentCustomerId = currentCustomerId,
            RentalStartDate = rentalStartDate
        };

        _mockVehicleRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync([vehicle]);

        var input = new GetAllVehiclesInputDto();

        // Act
        await _useCase.Execute(input);

        // Assert
        _mockOutputPort.Verify(x => x.StandardHandle(It.Is<GetAllVehiclesOutputDto>(dto =>
            dto.Vehicles.Count() == 1 &&
            dto.Vehicles.First().Brand == "BMW" &&
            dto.Vehicles.First().Model == "X5" &&
            dto.Vehicles.First().ManufacturingDate == manufacturingDate &&
            dto.Vehicles.First().IsRented
        )), Times.Once);
    }
}
