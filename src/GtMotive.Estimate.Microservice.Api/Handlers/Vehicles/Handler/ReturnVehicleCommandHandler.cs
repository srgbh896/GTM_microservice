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
public sealed class ReturnVehicleCommandHandler(
    ReturnVehicleUseCase useCase,
    GenericPresenter<ReturnVehicleOutputDto> presenter) : IRequestHandler<ReturnVehicleCommand, IWebApiPresenter>
{
    /// <summary>
    /// Handles the GetAllVehiclesRequest by delegating to the use case.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The presenter with the formatted HTTP response.</returns>
    public async Task<IWebApiPresenter> Handle(ReturnVehicleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var input = new ReturnVehicleInputDto
        {
            VehicleId = request.VehicleId,
            ReturnDate = request.ReturnDate
        };
        await useCase.Execute(input);
        return presenter;
    }
}
