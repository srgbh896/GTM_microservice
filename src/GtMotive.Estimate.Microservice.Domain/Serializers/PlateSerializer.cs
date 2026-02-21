using System;
using GtMotive.Estimate.Microservice.Domain.Entities.ValueObj;
using MongoDB.Bson.Serialization;

namespace GtMotive.Estimate.Microservice.Domain.Serializers;

/// <summary>
/// MongoDB BSON serializer for the Plate value object.
/// </summary>
public class PlateSerializer : IBsonSerializer<Plate>
{
    /// <summary>
    /// Gets the type this serializer handles.
    /// Required by MongoDB to know which CLR type this serializer supports.
    /// </summary>
    public Type ValueType => typeof(Plate);

    /// <summary>
    /// Deserializes a BSON string into a Plate value object.
    /// 
    /// Called when reading from MongoDB.
    /// Reads the string value and reconstructs the Plate
    /// using the domain factory method to preserve invariants.
    /// </summary>
    public Plate Deserialize(
        BsonDeserializationContext context,
        BsonDeserializationArgs args)
    {
        ArgumentNullException.ThrowIfNull(context);
        var value = context.Reader.ReadString();
        return Plate.Create(value);
    }

    /// <summary>
    /// Serializes a Plate value object into a BSON string.
    /// 
    /// Called when saving to MongoDB.
    /// Writes only the primitive string value.
    /// </summary>
    public void Serialize(
        BsonSerializationContext context,
        BsonSerializationArgs args,
        Plate value)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(value);
        context.Writer.WriteString(value.Value);
    }

    /// <summary>
    /// Non-generic serialization method required by IBsonSerializer.
    /// Handles object-based serialization by validating the type
    /// and delegating to the strongly-typed serializer logic.
    /// </summary>
    public void Serialize(
        BsonSerializationContext context,
        BsonSerializationArgs args,
        object value)
    {
        if (value is Plate plate)
        {
            ArgumentNullException.ThrowIfNull(context);
            context.Writer.WriteString(plate.Value);
        }
        else
        {
            throw new NotSupportedException("The provided value is not a Plate instance.");
        }
    }

    /// <summary>
    /// Explicit interface implementation for non-generic deserialization.
    /// Delegates to the strongly-typed Deserialize method.
    /// </summary>
    object IBsonSerializer.Deserialize(
        BsonDeserializationContext context,
        BsonDeserializationArgs args)
    {
        ArgumentNullException.ThrowIfNull(context);
        var value = context.Reader.ReadString();
        return Plate.Create(value);
    }
}
