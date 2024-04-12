using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Zio.Features.Auditing;
using Zio.Features.Core;
using Zio.Features.Core.DependencyInjection;
using Zio.Features.Core.Timing;
using Zio.Features.Data.Filters;
using Zio.Features.Ddd.Domain.Entities;
using Zio.Features.Ddd.Domain.Events;
using Zio.Features.EntityFrameworkCore.EntityHistory;
using Zio.Features.Guids;

namespace Zio.Features.EntityFrameworkCore;

public class ZioDbContext<TDbContext> : DbContext, IZioEfCoreDbContext, ITransientDependency
    where TDbContext : DbContext
{

    public ILazyServiceProvider LazyServiceProvider { get; set; } = default!;

    protected virtual bool IsSoftDeleteFilterEnabled => DataFilter?.IsEnabled<ISoftDelete>() ?? false;

    public IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);

    public IDataFilter DataFilter => LazyServiceProvider.LazyGetRequiredService<IDataFilter>();

    public IEntityChangeEventHelper EntityChangeEventHelper => LazyServiceProvider.LazyGetService<IEntityChangeEventHelper>(NullEntityChangeEventHelper.Instance);

    public IAuditPropertySetter AuditPropertySetter => LazyServiceProvider.LazyGetRequiredService<IAuditPropertySetter>();
    
    public IEntityHistoryHelper EntityHistoryHelper => LazyServiceProvider.LazyGetService<IEntityHistoryHelper>(NullEntityHistoryHelper.Instance);
    
    public IClock Clock => LazyServiceProvider.LazyGetRequiredService<IClock>();
    
    public ILogger<ZioDbContext<TDbContext>> Logger => LazyServiceProvider.LazyGetService<ILogger<ZioDbContext<TDbContext>>>(NullLogger<ZioDbContext<TDbContext>>.Instance);
    
    public IOptions<ZioDbContextOptions> Options => LazyServiceProvider.LazyGetRequiredService<IOptions<ZioDbContextOptions>>();
    
    private static readonly MethodInfo ConfigureBasePropertiesMethodInfo
        = typeof(ZioDbContext<TDbContext>)
            .GetMethod(
                nameof(ConfigureBaseProperties),
                BindingFlags.Instance | BindingFlags.NonPublic
            )!;
    
    public Task<int> SaveChangesOnDbContextAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
    }

    public void Initialize(ZioEfCoreDbContextInitializationContext initializationContext)
    {
        throw new NotImplementedException();
    }
    
    protected virtual void ConfigureBaseProperties<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        where TEntity : class
    {
        if (mutableEntityType.IsOwned())
        {
            return;
        }

        if (!typeof(IEntity).IsAssignableFrom(typeof(TEntity)))
        {
            return;
        }

        modelBuilder.Entity<TEntity>().ConfigureByConvention();

        ConfigureGlobalFilters<TEntity>(modelBuilder, mutableEntityType);
    }
}