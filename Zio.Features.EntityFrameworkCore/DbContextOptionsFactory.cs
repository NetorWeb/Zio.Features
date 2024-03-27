using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Zio.Features.Data;
using Zio.Features.Data.Abstraction;
using Zio.Features.Data.Options;
using Zio.Features.EntityFrameworkCore.Options;

namespace Zio.Features.EntityFrameworkCore;

public static class DbContextOptionsFactory
{
    public static DbContextOptions<TDbContext> Create<TDbContext>(IServiceProvider serviceProvider)
        where TDbContext : ZioDbContext<TDbContext>
    {
        var creationContext = GetCreationContext<TDbContext>(serviceProvider);

        var context = new ZioDbContextConfigurationContext<TDbContext>(
            serviceProvider,
            creationContext.ConnectionString,
            creationContext.ConnectionStringName,
            creationContext.ExistingConnection
        );

        var options = GetDbContextOptions<TDbContext>(serviceProvider);

        Configure(options, context);

        return context.DbContextOptions.Options;
    }

    private static void Configure<TDbContext>(
        ZioDbContextOptions options,
        ZioDbContextConfigurationContext<TDbContext> context
    ) where TDbContext : ZioDbContext<TDbContext>
    {
        var configureAction = options.ConfigureActions.GetValueOrDefault(typeof(TDbContext));
        if (configureAction != null)
        {
            ((Action<ZioDbContextConfigurationContext<TDbContext>>)configureAction).Invoke(context);
        }
        else if (options.DefaultConfigureAction != null)
        {
            options.DefaultConfigureAction.Invoke(context);
        }
        else
        {
            throw new Exception(
                $"No configuration found for {typeof(DbContext).AssemblyQualifiedName}! Use services.Configure<ZioDbContextOptions>(...) to configure it."
            );
        }
    }

    private static ZioDbContextOptions GetDbContextOptions<TDbContext>(IServiceProvider serviceProvider)
        where TDbContext : ZioDbContext<TDbContext> =>
        serviceProvider.GetRequiredService<IOptions<ZioDbContextOptions>>().Value;

    private static DbContextCreationContext GetCreationContext<TDbContext>(IServiceProvider serviceProvider)
    {
        var context = DbContextCreationContext.Current;
        if (context != null)
        {
            return context;
        }

        var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();
        var connectionString = ResolveConnectionString<TDbContext>(serviceProvider, connectionStringName);

        return new DbContextCreationContext(
            connectionStringName,
            connectionString
        );
    }

    private static string ResolveConnectionString<TDbContext>(
        IServiceProvider serviceProvider,
        string connectionStringName)
    {
        var connectionStringResolver = serviceProvider.GetRequiredService<IConnectionStringResolver>();

        return connectionStringResolver.Resolve(connectionStringName);
    }
}