using AutoMapper;
using GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Entities.ValueObj;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Profiles;

/// <summary>
/// Vehicle mapper profile
/// </summary>
public class VehicleProfile : Profile
{
    /// <summary>
    /// vehicle Profile constructor
    /// </summary>
    public VehicleProfile()
    {
        CreateMap<Vehicle, VehicleDto>()
            .ForMember(dest => dest.LicensePlate, opt => opt.MapFrom(src => Plate.Create(src.LicensePlate.ToString())));
        CreateMap<CreateVehicleInputDto, Vehicle>()
            .ForMember(dest => dest.LicensePlate, opt => opt.MapFrom(src => Plate.Create(src.LicensePlate)));
    }
}
