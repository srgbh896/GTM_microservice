using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Features.Vehicles.GetAllVehicles;
using GtMotive.Estimate.Microservice.Api.Presenters.Vehicles;
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.UseCase;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.Handlers.Vehicles.Handler;

/// <summary>
/// Handler for the CreateVehicleCommandHandler.
/// Acts as the adapter between the MediatR mediator and the use case.
/// </summary>
public sealed class CreateVehicleCommandHandler(
    CreateVehicleUseCase useCase,
    GenericPresenter<CreateVehicleOutputDto> presenter) : IRequestHandler<CreateVehicleCommand, IWebApiPresenter>
{
    /// <summary>
    /// Handles the GetAllVehiclesRequest by delegating to the use case.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The presenter with the formatted HTTP response.</returns>
    public async Task<IWebApiPresenter> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var input = new CreateVehicleInputDto()
        {
            Brand = request.Brand,
            Model = request.Model,
            LicensePlate = request.LicensePlate,
            ManufacturingDate = request.ManufacturingDate.Value
        };
        await useCase.Execute(input);
        return presenter;
    }
}
