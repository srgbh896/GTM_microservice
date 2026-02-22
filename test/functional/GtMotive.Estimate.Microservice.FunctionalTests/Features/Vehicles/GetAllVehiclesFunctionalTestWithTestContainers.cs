using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Features.Vehicles.GetAllVehicles;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Entities.ValueObj;
using GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Features.Vehicles;

/// <summary>
/// Collection definition for TestContainers-based GetAllVehicles tests.
/// Ensures that each test using this collection gets its own isolated MongoDB container.
/// </summary>
[CollectionDefinition("GetAllVehicles-TestContainers")]
[SuppressMessage("Design", "CA1515:Consider making public types internal")]
[SuppressMessage("Naming", "CA1711:Remove 'Collection' suffix")]
public class GetAllVehiclesTestContainersCollectionDefinition : ICollectionFixture<CompositionRootTestFixtureWithTestcontainers>
{
}

/// <summary>
/// Functional tests for the GetAllVehicles use case using TestContainers.
/// These tests verify the complete integration flow from the request handler through the use case to a containerized MongoDB,
/// excluding the HTTP Host layer.
/// Each test has its own isolated MongoDB container, ensuring test independence and reproducibility.
/// </summary>
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
[Collection("GetAllVehicles-TestContainers")]
public sealed class GetAllVehiclesFunctionalTestWithTestContainers(CompositionRootTestFixtureWithTestcontainers fixture) : IAsyncLifetime
{
    private readonly CompositionRootTestFixtureWithTestcontainers _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));

    /// <summary>
    /// Initialize async lifecycle - called before first test in collection.
    /// Starts the MongoDB container and builds the DI container.
    /// </summary>
    public async Task InitializeAsync()
    {
        await _fixture.InitializeAsync();
    }

    /// <summary>
    /// Dispose async lifecycle - called after all tests in collection.
    /// Stops the MongoDB container and cleans up resources.
    /// </summary>
    public async Task DisposeAsync()
    {
        await _fixture.DisposeAsync();
    }

    [Fact]
    public async Task GetAllVehicles_ShouldReturnListOfVehicles_WhenDatabaseContainsVehicles()
    {
        // Arrange - Seed test data into the isolated container
        var testVehicles = new List<Vehicle>
        {
            new()
            {
                Brand = "Toyota",
                Model = "Corolla",
                LicensePlate = Plate.Create("test002"),
                ManufacturingDate = DateTime.UtcNow.AddYears(-2),
                IsRented = false
            },
            new()
            {
                Brand = "Honda",
                Model = "Civic",
                LicensePlate = Plate.Create("test001"),
                ManufacturingDate = DateTime.UtcNow.AddYears(-1),
                IsRented = true,
                RentalStartDate = DateTime.UtcNow.AddDays(-5)
            }
        };

        await fixture.SeedDatabaseAsync(testVehicles, "vehicles");


        var request = new GetAllVehiclesRequest();

        // Act
        IWebApiPresenter presenter = null;
        await _fixture.UsingHandlerForRequestResponse<GetAllVehiclesRequest, IWebApiPresenter>(async handler =>
        {
            presenter = await handler.Handle(request, default);
        });
        // Assert
        Assert.NotNull(presenter);
        Assert.NotNull(presenter.ActionResult);

        var okResult = Assert.IsType<OkObjectResult>(presenter.ActionResult);
        Assert.NotNull(okResult.Value);

        var vehicles = okResult.Value as IEnumerable<VehicleOutputDto>;
        Assert.NotNull(vehicles);
        Assert.NotEmpty(vehicles);

        var vehicleList = vehicles.ToList();
        Assert.True(vehicleList.All(v => v.Id != Guid.Empty));
        Assert.True(vehicleList.All(v => !string.IsNullOrWhiteSpace(v.Brand)));
        Assert.True(vehicleList.All(v => !string.IsNullOrWhiteSpace(v.Model)));
        Assert.True(vehicleList.All(v => !string.IsNullOrWhiteSpace(v.LicensePlate)));

        // Verify we got exactly the seeded data
        Assert.Equal(2, vehicleList.Count);
    }

    [Fact]
    public async Task GetAllVehicles_ShouldReturnEmptyList_WhenNoVehiclesExist()
    {
        // Arrange - Ensure database is empty for this test
        await _fixture.SeedDatabaseAsync(new List<Vehicle>(), "vehicles", true);

        var request = new GetAllVehiclesRequest();

        // Act
        IWebApiPresenter presenter = null;
        await _fixture.UsingHandlerForRequestResponse<GetAllVehiclesRequest, IWebApiPresenter>(async handler =>
        {
            presenter = await handler.Handle(request, default);
        });

        // Assert
        Assert.NotNull(presenter);
        var okResult = Assert.IsType<OkObjectResult>(presenter.ActionResult);
        var vehicles = okResult.Value as IEnumerable<VehicleOutputDto>;
        Assert.NotNull(vehicles);
        Assert.Empty(vehicles); // Should be truly empty now
    }
}
