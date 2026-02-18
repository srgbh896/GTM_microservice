using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GtMotive.Estimate.Microservice.Domain.Base;

public abstract class IDocument
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityBase"/> class.
    /// </summary>
    protected IDocument()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Gets Id.
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; private set; }

    [BsonElement()]
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
}
