using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Features.Vehicles.GetAllVehicles;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;
using GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Features.Vehicles;

/// <summary>
/// Functional tests for the GetAllVehicles use case.
/// These tests verify the complete integration flow from the request handler through the use case to the database,
/// excluding the HTTP Host layer.
/// </summary>
public sealed class GetAllVehiclesFunctionalTest(CompositionRootTestFixture fixture) : FunctionalTestBase(fixture)
{
    [Fact]
    public async Task GetAllVehiclesShouldReturnListOfVehicleWhenDatabaseContainsVehicles()
    {
        // Arrange
        var request = new GetAllVehiclesRequest();
        // Act
        IWebApiPresenter presenter = null;

        await Fixture.UsingHandlerForRequestResponse<GetAllVehiclesRequest, IWebApiPresenter>(async handler =>
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
    }
}

