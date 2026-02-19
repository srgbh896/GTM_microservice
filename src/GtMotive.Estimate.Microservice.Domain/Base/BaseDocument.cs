using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GtMotive.Estimate.Microservice.Domain.Base;

/// <summary>
/// IDocument base class
/// </summary>
public abstract class BaseDocument
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseDocument"/> class.
    /// </summary>
    protected BaseDocument()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Gets Id.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    /// <remarks>This property is typically set automatically when a new entity instance is created. It is
    /// useful for auditing and tracking the creation time of records.</remarks>
    [BsonElement()]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was last updated.
    /// </summary>
    /// <remarks>This property is typically used to track the last modification time of the entity, which can
    /// be useful for auditing and synchronization purposes.</remarks>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the name of the user who created the entity.
    /// </summary>
    /// <remarks>This property is typically set during the creation of the entity and may be used for auditing
    /// purposes.</remarks>
    public string CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last updated the record.
    /// </summary>
    public string UpdatedBy { get; set; }
}
