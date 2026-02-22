using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Features.Vehicles.GetAllVehicles;
using GtMotive.Estimate.Microservice.Api.Presenters.Vehicles;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.UseCase;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.Handlers.Vehicles;

/// <summary>
/// Handler for the CreateVehicleCommandHandler.
/// Acts as the adapter between the MediatR mediator and the use case.
/// </summary>
public sealed class RentVehicleCommandHandler(
    RentVehicleUseCase useCase,
    GenericPresenter<RentVehicleOutputDto> presenter) : IRequestHandler<RentVehicleCommand, IWebApiPresenter>
{
    /// <summary>
    /// Handles the GetAllVehiclesRequest by delegating to the use case.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The presenter with the formatted HTTP response.</returns>
    public async Task<IWebApiPresenter> Handle(RentVehicleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inputDto = new RentVehicleInputDto
        {
            VehicleId = request.VehicleId,
            CustomerIdentifier = request.CustomerIdentifier,
            PlannedReturnDate = request.PlannedReturnDate.Value,
            StartDate = request.StartDate
        };
        await useCase.Execute(inputDto);
        return presenter;
    }
}
