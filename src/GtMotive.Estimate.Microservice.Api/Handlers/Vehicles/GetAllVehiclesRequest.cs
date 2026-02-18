using GtMotive.Estimate.Microservice.Api.UseCases;
using MediatR;

namespace GtMotive.Estimate.Microservice.Api.Features.Vehicles.GetAllVehicles;

/// <summary>
/// Represents a request to retrieve all vehicles.
/// This is the MediatR request object that will be dispatched through the mediator.
/// </summary>
public sealed class GetAllVehiclesRequest : IRequest<IWebApiPresenter>
{
}
