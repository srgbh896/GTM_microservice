using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb;

/// <summary>
/// Servicio de MongoDB que proporciona acceso al cliente y base de datos de MongoDB.
/// </summary>
public class MongoService
{
    /// <summary>
    /// Obtiene el cliente de MongoDB configurado.
    /// </summary>
    public MongoClient MongoClient { get; }

    /// <summary>
    /// Obtiene la instancia de la base de datos de MongoDB.
    /// </summary>
    public IMongoDatabase Database { get; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MongoService"/>.
    /// </summary>
    /// <param name="options">Las opciones de configuración de MongoDB.</param>
    public MongoService(IOptions<MongoDbSettings> options)
    {
        var settings = options.Value;

        MongoClient = new MongoClient(settings.ConnectionString);
        Database = MongoClient.GetDatabase(settings.MongoDbDatabaseName);
    }
}
