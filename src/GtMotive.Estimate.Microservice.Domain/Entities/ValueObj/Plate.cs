using System;
using System.Collections.Generic;
using GtMotive.Estimate.Microservice.Domain.Base;
using GtMotive.Estimate.Microservice.Domain.Serializers;
using MongoDB.Bson.Serialization.Attributes;

namespace GtMotive.Estimate.Microservice.Domain.Entities.ValueObj;

/// <summary>
/// License plate value object
/// </summary>
[BsonSerializer(typeof(PlateSerializer))]
public sealed class Plate : ValueObject
{
    /// <summary>
    /// License plate value
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Private const.
    /// </summary>
    /// <param name="value"></param>
    private Plate(string value)
    {
        Value = value;
    }

    /// <summary>
    /// C
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static Plate Create(string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("License plate cannot be empty.")
            : new Plate(value.ToUpper(System.Globalization.CultureInfo.CurrentCulture));
    }

    /// <summary>
    /// Get string value of license plate
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value;

    /// <summary>
    /// Get the components that define equality for this value object. In this case, it's just the Value property.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
