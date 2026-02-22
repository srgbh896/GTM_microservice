using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using GtMotive.Estimate.Microservice.Api.Authorization;
using GtMotive.Estimate.Microservice.Api.DependencyInjection;
using GtMotive.Estimate.Microservice.Api.Filters;
using GtMotive.Estimate.Microservice.Api.Presenters.Vehicles;
using GtMotive.Estimate.Microservice.ApplicationCore;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto.Base;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

[assembly: CLSCompliant(false)]

namespace GtMotive.Estimate.Microservice.Api;

[ExcludeFromCodeCoverage]
public static class ApiConfiguration
{
    public static void ConfigureControllers(MvcOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        options.Filters.Add<BusinessExceptionFilter>();
    }

    public static IMvcBuilder WithApiControllers(this IMvcBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddApplicationPart(typeof(ApiConfiguration).GetTypeInfo().Assembly);

        AddApiDependencies(builder.Services);

        return builder;
    }

    public static void AddApiDependencies(this IServiceCollection services)
    {
        services.AddAuthorization(AuthorizationOptionsExtensions.Configure);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApiConfiguration).GetTypeInfo().Assembly));
        services.AddUseCases();
        services.AddPresenters();

        services.AddSingleton<IOutputPortStandard<Result<IEnumerable<VehicleOutputDto>>>>(sp => sp.GetRequiredService<GenericPresenter<IEnumerable<VehicleOutputDto>>>());
        services.AddSingleton<IOutputPortStandard<Result<CreateVehicleOutputDto>>>(sp => sp.GetRequiredService<GenericPresenter<CreateVehicleOutputDto>>());
        services.AddSingleton<IOutputPortStandard<Result<RentVehicleOutputDto>>>(
            sp => sp.GetRequiredService<GenericPresenter<RentVehicleOutputDto>>()
        );
        services.AddSingleton<IOutputPortStandard<Result<ReturnVehicleOutputDto>>>(sp => sp.GetRequiredService<GenericPresenter<ReturnVehicleOutputDto>>());
    }
}
