using System.Collections.Generic;
using GtMotive.Estimate.Microservice.Api.Presenters.Vehicles;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Api.DependencyInjection;

public static class UserInterfaceExtensions
{
    public static IServiceCollection AddPresenters(this IServiceCollection services)
    {
        services.AddSingleton<GenericPresenter<RentVehicleOutputDto>>();
        services.AddSingleton<GenericPresenter<IEnumerable<VehicleOutputDto>>>();
        services.AddSingleton<GenericPresenter<CreateVehicleOutputDto>>();
        services.AddSingleton<GenericPresenter<ReturnVehicleOutputDto>>();
        return services;
    }
}
