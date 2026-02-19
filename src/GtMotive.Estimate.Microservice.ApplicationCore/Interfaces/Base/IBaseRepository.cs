using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Base;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Interfaces.Base;

/// <summary>
/// Interface que define las operaciones CRUD básicas para repositorios genéricos.
/// </summary>
/// <typeparam name="TDocument">El tipo de documento que hereda de BaseDocument.</typeparam>
public interface IBaseRepository<TDocument>
    where TDocument : BaseDocument
{
    /// <summary>
    /// Obtiene un documento por su identificador.
    /// </summary>
    /// <param name="id">El identificador único del documento.</param>
    /// <returns>El documento encontrado, o null si no existe.</returns>
    Task<TDocument> GetByIdAsync(Guid id);

    /// <summary>
    /// Obtiene todos los documentos de la colección.
    /// </summary>
    /// <returns>Una colección de solo lectura con todos los documentos.</returns>
    Task<IReadOnlyCollection<TDocument>> GetAllAsync();

    /// <summary>
    /// Filtra documentos según una expresión de predicado.
    /// </summary>
    /// <param name="filterExpression">La expresión LINQ para filtrar documentos.</param>
    /// <returns>Una colección de solo lectura con los documentos que coinciden el filtro.</returns>
    Task<IReadOnlyCollection<TDocument>> FilterByAsync(
        Expression<Func<TDocument, bool>> filterExpression);

    /// <summary>
    /// Inserta un nuevo documento en la colección.
    /// </summary>
    /// <param name="document">El documento a insertar.</param>
    Task InsertOneAsync(TDocument document);

    /// <summary>
    /// Elimina un documento por su identificador.
    /// </summary>
    /// <param name="id">El identificador único del documento a eliminar.</param>
    Task DeleteByIdAsync(Guid id);
}
