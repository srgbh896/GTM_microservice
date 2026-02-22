using System;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto.Base;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Presenters.Vehicles;

/// <summary>
/// Presenter for the GetAllVehicles use case.
/// Implements both IWebApiPresenter and IOutputPortStandard to bridge between the use case and the HTTP response.
/// </summary>
public sealed class GenericPresenter<T> : IWebApiPresenter, IOutputPortStandard<Result<T>>
{
    /// <summary>
    /// Gets the HTTP action result that will be returned to the client.
    /// </summary>
    public IActionResult ActionResult { get; private set; }

    /// <summary>
    /// Handles the output from the use case and formats it for the HTTP response.
    /// </summary>
    /// <param name="response">The output from the GetAllVehicles use case.</param>
    public void StandardHandle(Result<T> response)
    {
        ArgumentNullException.ThrowIfNull(response);
        if (response.IsSuccess)
        {
            ActionResult = new OkObjectResult(response.Value);
        }

        if (response.IsFailure)
        {
            ActionResult = new BadRequestObjectResult(response.Error);
        }
    }
}

