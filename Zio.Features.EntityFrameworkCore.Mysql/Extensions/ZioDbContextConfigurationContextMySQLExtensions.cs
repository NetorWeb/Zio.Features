using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Zio.Features.EntityFrameworkCore.Mysql.Extensions;

public static class ZioDbContextConfigurationContextMySQLExtensions
{
    public static DbContextOptionsBuilder UseMySql(
        this ZioDbContextConfigurationContext context,
        Action<MySqlDbContextOptionsBuilder>? mysqlOptionsAction = null)
    {
        if (context.ExistingConnection != null)
        {
            return context.DbContextOptions.UseMySql(context.ExistingConnection,
                ServerVersion.AutoDetect(context.ConnectionString), builder =>
                {
                    builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    mysqlOptionsAction?.Invoke(builder);
                }
            );
        }
        else
        {
            return context.DbContextOptions.UseMySql(context.ConnectionString,
                ServerVersion.AutoDetect(context.ConnectionString), builder =>
                {
                    builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    mysqlOptionsAction?.Invoke(builder);
                }
            );
        }
    }
}