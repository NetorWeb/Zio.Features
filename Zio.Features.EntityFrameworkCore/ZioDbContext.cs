using Microsoft.EntityFrameworkCore;
using Zio.Features.Core.DependencyInjection;
using Zio.Features.EntityFrameworkCore.Extensions;

namespace Zio.Features.EntityFrameworkCore;

/// <summary>
/// 核心数据库上下文
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
public class ZioDbContext<TDbContext> : DbContext
    where TDbContext : DbContext
{
    protected ZioDbContext(DbContextOptions<TDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        TrySetDatabaseProvider(modelBuilder);
    }

    private void TrySetDatabaseProvider(ModelBuilder modelBuilder)
    {
        var provider = GetDatabaseProviderOrNull(modelBuilder);
        if (provider != null)
        {
            modelBuilder.SetDatabaseProvider(provider.Value);
        }
    }

    private EfCoreDatabaseProvider? GetDatabaseProviderOrNull(ModelBuilder modelBuilder)
    {
        switch (Database.ProviderName)
        {
            case "Microsoft.EntityFrameworkCore.SqlServer":
                return EfCoreDatabaseProvider.SqlServer;
            case "Npgsql.EntityFrameworkCore.PostgreSQL":
                return EfCoreDatabaseProvider.PostgreSql;
            case "Pomelo.EntityFrameworkCore.MySql":
                return EfCoreDatabaseProvider.MySql;
            case "Oracle.EntityFrameworkCore":
            case "Devart.Data.Oracle.Entity.EFCore":
                return EfCoreDatabaseProvider.Oracle;
            case "Microsoft.EntityFrameworkCore.InMemory":
                return EfCoreDatabaseProvider.InMemory;
            default:
                return null;
        }
    }
}