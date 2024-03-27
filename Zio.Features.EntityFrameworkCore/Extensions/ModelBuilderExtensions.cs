using Microsoft.EntityFrameworkCore;

namespace Zio.Features.EntityFrameworkCore.Extensions;

public static class ModelBuilderExtensions
{
    private const string ModelDatabaseProviderAnnotationKey = "_Abp_DatabaseProvider";
    private const string ModelMultiTenancySideAnnotationKey = "_Abp_MultiTenancySide";

    public static void SetDatabaseProvider(this ModelBuilder modelBuilder, EfCoreDatabaseProvider provider)
    {
        modelBuilder.Model.SetAnnotation(ModelDatabaseProviderAnnotationKey, provider);
    }

    public static void ClearDatabaseProvider(this ModelBuilder modelBuilder)
    {
        modelBuilder.Model.RemoveAnnotation(ModelDatabaseProviderAnnotationKey);
    }

    public static EfCoreDatabaseProvider? GetDatabaseProvider(this ModelBuilder modelBuilder)
    {
        return (EfCoreDatabaseProvider?)modelBuilder.Model[ModelDatabaseProviderAnnotationKey];
    }

    public static bool IsUsingMySQL(
        this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetDatabaseProvider() == EfCoreDatabaseProvider.MySql;
    }

    public static bool IsUsingOracle(
        this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetDatabaseProvider() == EfCoreDatabaseProvider.Oracle;
    }

    public static bool IsUsingSqlServer(
        this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetDatabaseProvider() == EfCoreDatabaseProvider.SqlServer;
    }

    public static bool IsUsingPostgreSql(
        this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetDatabaseProvider() == EfCoreDatabaseProvider.PostgreSql;
    }

    public static void UseInMemory(
        this ModelBuilder modelBuilder)
    {
        modelBuilder.SetDatabaseProvider(EfCoreDatabaseProvider.InMemory);
    }

    public static bool IsUsingInMemory(
        this ModelBuilder modelBuilder)
    {
        return modelBuilder.GetDatabaseProvider() == EfCoreDatabaseProvider.InMemory;
    }
}