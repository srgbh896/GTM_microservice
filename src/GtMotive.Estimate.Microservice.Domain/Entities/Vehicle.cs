using System;
using GtMotive.Estimate.Microservice.Domain.Base;

namespace GtMotive.Estimate.Microservice.Domain.Entities;

public class Vehicle : IDocument
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public string LicensePlate { get; set; }

    public DateTime ManufacturingDate { get; set; }

    public bool IsRented { get; set; }
    public Guid? CurrentCustomerId { get; set; }
    public DateTime? RentalStartDate { get; set; }
}
