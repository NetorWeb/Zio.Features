using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Zio.Features.Data.Linq;
using Zio.Features.Ddd.Domain.Entities;

namespace Zio.Features.Ddd.Domain.Repositories;

public interface IReadOnlyRepository<TEntity> : IReadOnlyBasicRepository<TEntity>
    where TEntity : class, IEntity
{
    IAsyncQueryableExecutor AsyncExecuter { get; }

    IQueryable<TEntity> WithDetails();

    IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] propertySelectors);

    Task<IQueryable<TEntity>> WithDetailsAsync();

    Task<IQueryable<TEntity>> GetQueryableAsync();

    Task<List<TEntity>> GetListAsync(
        [NotNull] Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);
}

public interface IReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity>, IReadOnlyBasicRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{

}