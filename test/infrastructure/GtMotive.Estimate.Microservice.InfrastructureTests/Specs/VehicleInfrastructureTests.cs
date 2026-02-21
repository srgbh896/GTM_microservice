using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Specs;

/// <summary>
/// Integration tests for the Vehicle API endpoints.
/// 
/// These tests use a real test server and verify HTTP responses.
/// The focus is on the HTTP layer, model validation, and correct status codes.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="VehicleInfrastructureTests"/> class.
/// Uses a shared test server fixture provided by <see cref="GenericInfrastructureTestServerFixture"/>.
/// </remarks>
/// <param name="fixture">The shared test server fixture.</param>
[Collection(TestCollections.TestServer)]
public class VehicleInfrastructureTests(GenericInfrastructureTestServerFixture fixture) : InfrastructureTestBase(fixture)
{

    /// <summary>
    /// Tests that posting a vehicle with an invalid model returns HTTP 400 Bad Request.
    /// 
    /// The payload uses an empty license plate, which violates the domain Value Object rules.
    /// </summary>
    [Fact(DisplayName = "POST /api/vehicle - invalid model returns BadRequest")]
    public async Task PostVehicleShouldReturnBadRequestWhenModelIsInvalid()
    {
        await Fixture.InitializeAsync();

        // Arrange
        var client = Fixture.Server.CreateClient();

        var invalidPayload = new
        {
            plateNumber = string.Empty,
            manufacturedDate = "2024-01-01"
        };

        var json = JsonSerializer.Serialize(invalidPayload);

        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(
            new Uri("http://localhost/api/Vehicle"),
            content);
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest, "Empty plate violation");
    }

    /// <summary>
    /// Tests that posting a vehicle with a valid model returns HTTP 200 OK.
    /// 
    /// This verifies that valid input passes model validation and reaches the application layer.
    /// </summary>
    [Fact(DisplayName = "POST /api/vehicle - valid model returns OK")]
    public async Task PostVehicleShouldReturnSuccessWhenModelIsValid()
    {
        await Fixture.InitializeAsync();

        // Arrange
        var client = Fixture.Server.CreateClient();

        var validPayload = new
        {
            LicensePlate = "1234ABC",
            ManufacturingDate = DateTime.Now,
            Model = "testMOdel",
            Brand = "testBrand"
        };

        // Act
        var json = JsonSerializer.Serialize(validPayload);

        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        // Act
        var response = await client.PostAsync(
            new Uri("http://localhost/api/Vehicle"),
            content);
        var body = await response.Content.ReadAsStringAsync();
        Console.WriteLine(body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK, "valid input should pass validation and be accepted by the API");
    }
}
