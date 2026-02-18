using System;
using System.Collections.Generic;
using System.Text;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Features.Vehicles.Dto;

public class VehicleDto
{
    public Guid Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string LicensePlate { get; set; }
    public DateTime ManufacturingDate { get; set; }
    public bool IsRented { get; set; }
}
