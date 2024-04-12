using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zio.Features.Core.DependencyInjection;

namespace Zio.Features.EntityFrameworkCore.DependencyInjection;

public class ZioDbContextConfigurationContext: IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }
    
    public string ConnectionString { get; }

    public string? ConnectionStringName { get; }

    public DbConnection? ExistingConnection { get; }
    
    public DbContextOptionsBuilder DbContextOptions { get; protected set; }
    
    public ZioDbContextConfigurationContext(
        [NotNull] string connectionString,
        [NotNull] IServiceProvider serviceProvider,
        string? connectionStringName,
        DbConnection? existingConnection)
    {
        ConnectionString = connectionString;
        ServiceProvider = serviceProvider;
        ConnectionStringName = connectionStringName;
        ExistingConnection = existingConnection;

        DbContextOptions = new DbContextOptionsBuilder()
            .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
            .UseApplicationServiceProvider(serviceProvider);
    }
}

public class ZioDbContextConfigurationContext<TDbContext> : ZioDbContextConfigurationContext
    where TDbContext : ZioDbContext<TDbContext>
{
    public new DbContextOptionsBuilder<TDbContext> DbContextOptions => (DbContextOptionsBuilder<TDbContext>)base.DbContextOptions;

    public ZioDbContextConfigurationContext(
        string connectionString,
        [NotNull] IServiceProvider serviceProvider,
        string? connectionStringName,
        DbConnection? existingConnection)
        : base(
            connectionString,
            serviceProvider,
            connectionStringName,
            existingConnection)
    {
        base.DbContextOptions = new DbContextOptionsBuilder<TDbContext>()
            .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
            .UseApplicationServiceProvider(serviceProvider); ;
    }
}