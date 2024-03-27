using System.Diagnostics.CodeAnalysis;
using Zio.Features.Core;

namespace Zio.Features.EntityFrameworkCore.Options;

public class ZioDbContextOptions
{
    internal Dictionary<Type, object>? ConfigureActions { get; }

    internal Action<ZioDbContextConfigurationContext>? DefaultConfigureAction { get; set; }

    public ZioDbContextOptions()
    {
        ConfigureActions = new Dictionary<Type, object>();
    }

    public void Configure([NotNull] Action<ZioDbContextConfigurationContext> action)
    {
        Check.NotNull(action, nameof(action));

        DefaultConfigureAction = action;
    }

    public void Configure<TDbContext>([NotNull] Action<ZioDbContextConfigurationContext<TDbContext>> action)
        where TDbContext : ZioDbContext<TDbContext>
    {
        Check.NotNull(action, nameof(action));

        ConfigureActions[typeof(TDbContext)] = action;
    }

    public bool IsConfigured<TDbContext>()
    {
        return IsConfigured(typeof(TDbContext));
    }   

    public bool IsConfigured(Type dbContextType)
    {
        return ConfigureActions.ContainsKey(dbContextType);
    }
}