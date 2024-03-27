using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Zio.Features.EntityFrameworkCore;

public class ZioDbContextConfigurationContext
{
    public IServiceProvider ServiceProvider { get; }

    public string ConnectionString { get; }

    public string? ConnectionStringName { get; }

    public DbConnection? ExistingConnection { get; }

    public DbContextOptionsBuilder DbContextOptions { get; protected set; }

    public ZioDbContextConfigurationContext(
        IServiceProvider serviceProvider,
        string connectionString,
        string? connectionStringName,
        DbConnection? existingConnection
    )
    {
        ServiceProvider = serviceProvider;
        ConnectionString = connectionString;
        ConnectionStringName = connectionStringName;
        ExistingConnection = existingConnection;

        DbContextOptions = new DbContextOptionsBuilder()
            .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
            .UseApplicationServiceProvider(serviceProvider);
    }
}

public class ZioDbContextConfigurationContext<TDbContext> : ZioDbContextConfigurationContext
    where TDbContext : DbContext
{

    public new DbContextOptionsBuilder<TDbContext> DbContextOptions => (DbContextOptionsBuilder<TDbContext>)base.DbContextOptions;

    public ZioDbContextConfigurationContext(
        IServiceProvider serviceProvider,
        string connectionString,
        string? connectionStringName,
        DbConnection? existingConnection) : base(
        serviceProvider,
        connectionString,
        connectionStringName,
        existingConnection)
    {
        base.DbContextOptions = new DbContextOptionsBuilder<TDbContext>()
            .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
            .UseApplicationServiceProvider(serviceProvider);
    }
}