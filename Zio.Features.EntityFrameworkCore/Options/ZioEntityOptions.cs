using System.Diagnostics.CodeAnalysis;
using Zio.Features.Core;
using Zio.Features.Core.Extensions;
using Zio.Features.Ddd.Domain.Entities;

namespace Zio.Features.EntityFrameworkCore.Options;

public class ZioEntityOptions<TEntity>
where TEntity: IEntity
{
    public static ZioEntityOptions<TEntity> Empty { get; } = new ZioEntityOptions<TEntity>();

    public Func<IQueryable<TEntity>, IQueryable<TEntity>>? DefaultWithDetailsFunc { get; set; }
}

public class ZioEntityOptions
{
    private readonly IDictionary<Type, object> _options;

    public ZioEntityOptions()
    {
        _options = new Dictionary<Type, object>();
    }

    public ZioEntityOptions<TEntity>? GetOrNull<TEntity>()
        where TEntity : IEntity
    {
        return _options.GetOrDefault(typeof(TEntity)) as ZioEntityOptions<TEntity>;
    }

    public void Entity<TEntity>([NotNull] Action<ZioEntityOptions<TEntity>> optionsAction)
        where TEntity : IEntity
    {
        Check.NotNull(optionsAction, nameof(optionsAction));

        optionsAction(
            (_options.GetOrAdd(
                typeof(TEntity),
                () => new ZioEntityOptions<TEntity>()
            ) as ZioEntityOptions<TEntity>)!
        );
    }
}