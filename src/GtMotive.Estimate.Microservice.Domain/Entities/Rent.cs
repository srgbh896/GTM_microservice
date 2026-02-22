using System;
using GtMotive.Estimate.Microservice.Domain.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GtMotive.Estimate.Microservice.Domain.Entities;

/// <summary>
/// Representa un vehículo en el sistema de estimaciones.
/// </summary>
public class Rent : BaseDocument
{
    /// <summary>
    /// Start date of the rent.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Planned return date of the rent.
    /// </summary>
    public DateTime PlannedReturnDate { get; set; }

    /// <summary>
    /// Actual return date (null if not returned yet).
    /// </summary>
    public DateTime? ReturnDate { get; set; }

    /// <summary>
    /// Customer identifier.
    /// </summary>
    public string CustomerIdentifier { get; set; }

    /// <summary>
    /// Vehicle identifier.
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    public Guid VehicleId { get; set; }

    /// <summary>
    /// Indicates whether the vehicle has been returned.
    /// </summary>
    [BsonIgnore]
    public bool IsReturned => ReturnDate.HasValue;
}
