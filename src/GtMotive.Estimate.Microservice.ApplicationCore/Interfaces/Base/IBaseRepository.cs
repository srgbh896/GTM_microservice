using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Base;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Interfaces.Base;

public interface IBaseRepository<TDocument>
    where TDocument : IDocument
{
    Task<TDocument?> GetByIdAsync(Guid id);

    Task<IReadOnlyCollection<TDocument>> GetAllAsync();

    Task<IReadOnlyCollection<TDocument>> FilterByAsync(
        Expression<Func<TDocument, bool>> filterExpression);

    Task InsertOneAsync(TDocument document);

    Task DeleteByIdAsync(Guid id);
}
