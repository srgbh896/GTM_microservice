using System;
using GtMotive.Estimate.Microservice.Domain.Base;
using GtMotive.Estimate.Microservice.Domain.Entities.ValueObj;

namespace GtMotive.Estimate.Microservice.Domain.Entities;

/// <summary>
/// Representa un vehículo en el sistema de estimaciones.
/// </summary>
public class Vehicle : BaseDocument
{
    /// <summary>
    /// Obtiene o establece la marca del vehículo.
    /// </summary>
    public string Brand { get; set; }

    /// <summary>
    /// Obtiene o establece el modelo del vehículo.
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Obtiene o establece la matrícula del vehículo.
    /// </summary>
    public Plate LicensePlate { get; set; }

    /// <summary>
    /// Obtiene o establece la fecha de fabricación del vehículo.
    /// </summary>
    public DateTime ManufacturingDate { get; set; }

    /// <summary>
    /// Obtiene o establece un valor que indica si el vehículo está actualmente alquilado.
    /// </summary>
    public bool IsRented { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador único del cliente actual que alquila el vehículo.
    /// Es nulo si el vehículo no está alquilado.
    /// </summary>
    public Guid? CurrentCustomerId { get; set; }

    /// <summary>
    /// Obtiene o establece la fecha de inicio del alquiler actual del vehículo.
    /// Es nulo si el vehículo no está alquilado.
    /// </summary>
    public DateTime? RentalStartDate { get; set; }
}
