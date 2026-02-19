using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Interfaces.Base;
using GtMotive.Estimate.Microservice.Domain.Base;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.Base;

/// <summary>
/// Clase base abstracta que proporciona operaciones CRUD genéricas para documentos MongoDB.
/// </summary>
/// <typeparam name="TDocument">Tipo de documento que hereda de <see cref="BaseDocument"/>.</typeparam>
public abstract class RepositoryBase<TDocument>(MongoService service, string collectionName) : IBaseRepository<TDocument>
    where TDocument : BaseDocument
{
    /// <summary>
    /// Colección de MongoDB para operaciones de datos del tipo TDocument.
    /// </summary>
    internal readonly IMongoCollection<TDocument> _collection = service.Database.GetCollection<TDocument>(collectionName);

    /// <summary>
    /// Obtiene un documento por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del documento.</param>
    /// <returns>El documento encontrado o null si no existe.</returns>
    public async Task<TDocument> GetByIdAsync(Guid id)
    {
        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Obtiene todos los documentos de la colección.
    /// </summary>
    /// <returns>Una colección de solo lectura con todos los documentos.</returns>
    public async Task<IReadOnlyCollection<TDocument>> GetAllAsync()
    {
        return await _collection.Find(Builders<TDocument>.Filter.Empty)
                                .ToListAsync();
    }

    /// <summary>
    /// Obtiene documentos que cumplen con la expresión de filtro especificada.
    /// </summary>
    /// <param name="filterExpression">Expresión LINQ que define el filtro.</param>
    /// <returns>Una colección de solo lectura con los documentos que coinciden.</returns>
    public async Task<IReadOnlyCollection<TDocument>> FilterByAsync(
        Expression<Func<TDocument, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression)
                                .ToListAsync();
    }

    /// <summary>
    /// Inserta un nuevo documento en la colección.
    /// </summary>
    /// <param name="document">El documento a insertar.</param>
    public async Task InsertOneAsync(TDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);

        document.CreatedAt = DateTime.UtcNow;
        document.UpdatedAt = DateTime.UtcNow;

        await _collection.InsertOneAsync(document);
    }

    /// <summary>
    /// Reemplaza un documento existente en la colección.
    /// </summary>
    /// <param name="document">El documento con los datos actualizados.</param>
    public async Task ReplaceOneAsync(TDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);

        document.UpdatedAt = DateTime.UtcNow;

        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, document.Id);

        await _collection.ReplaceOneAsync(filter, document);
    }

    /// <summary>
    /// Elimina un documento por su identificador único.
    /// </summary>
    /// <param name="id">El identificador único del documento a eliminar.</param>
    public async Task DeleteByIdAsync(Guid id)
    {
        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
        await _collection.DeleteOneAsync(filter);
    }

    /// <summary>
    /// Verifica si existe un documento que cumpla la expresión de filtro especificada.
    /// </summary>
    /// <param name="filterExpression">Expresión LINQ que define el filtro.</param>
    /// <returns>true si existe al menos un documento que coincida; en caso contrario, false.</returns>
    public async Task<bool> ExistsAsync(
        Expression<Func<TDocument, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression).AnyAsync();
    }
}
