using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Interfaces.Base;
using GtMotive.Estimate.Microservice.Domain.Base;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.Base;

public abstract class RepositoryBase<TDocument>(MongoService service, string collectionName) : IBaseRepository<TDocument>
    where TDocument : IDocument
{
    internal readonly IMongoCollection<TDocument> _collection = service.Database.GetCollection<TDocument>(collectionName);

    public async Task<TDocument?> GetByIdAsync(Guid id)
    {
        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<TDocument>> GetAllAsync()
    {
        return await _collection.Find(Builders<TDocument>.Filter.Empty)
                                .ToListAsync();
    }

    public async Task<IReadOnlyCollection<TDocument>> FilterByAsync(
        Expression<Func<TDocument, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression)
                                .ToListAsync();
    }

    public async Task InsertOneAsync(TDocument document)
    {
        document.CreatedAt = DateTime.UtcNow;
        document.UpdatedAt = DateTime.UtcNow;

        await _collection.InsertOneAsync(document);
    }

    public async Task ReplaceOneAsync(TDocument document)
    {
        document.UpdatedAt = DateTime.UtcNow;

        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, document.Id);

        await _collection.ReplaceOneAsync(filter, document);
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
        await _collection.DeleteOneAsync(filter);
    }

    public async Task<bool> ExistsAsync(
        Expression<Func<TDocument, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression).AnyAsync();
    }
}
