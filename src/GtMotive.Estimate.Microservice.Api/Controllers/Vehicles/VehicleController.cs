using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Features.Vehicles.GetAllVehicles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Listar vehículos disponibles en la flota.
    /// </summary>
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableVehicles()
    {
        var presenter = await mediator.Send(new GetAllVehiclesRequest());
        return presenter.ActionResult;
    }

    /// <summary>
    /// Crear un nuevo vehículo en la flota.
    /// Restricción: No se permiten vehículos con más de 5 años desde su fecha de fabricación.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleCommand command)
    {
        var presenter = await mediator.Send(command);
        return presenter.ActionResult;
    }

    /// <summary>
    /// Rent vehicle method
    /// </summary>
    /// <param name="vehicleId">vehicle id</param>
    /// <param name="command">rent command</param>
    /// <returns></returns>
    [HttpPost("{vehicleId:guid}/rent")]
    public async Task<IActionResult> RentVehicle(Guid vehicleId, [FromBody] RentVehicleCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        command.VehicleId = vehicleId;
        var presenter = await mediator.Send(command);
        return presenter.ActionResult;
    }

    /// <summary>
    /// Devolver un vehículo.
    /// </summary>
    [HttpPost("{vehicleId:guid}/return")]
    public async Task<IActionResult> ReturnVehicle(Guid vehicleId)
    {
        var command = new ReturnVehicleCommand
        {
            VehicleId = vehicleId,
        };

        var presenter = await mediator.Send(command);
        return presenter.ActionResult;
    }
}
