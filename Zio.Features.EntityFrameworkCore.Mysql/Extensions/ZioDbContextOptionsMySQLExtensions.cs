using Microsoft.EntityFrameworkCore.Infrastructure;
using Zio.Features.EntityFrameworkCore.Options;

namespace Zio.Features.EntityFrameworkCore.Mysql.Extensions;

public static class ZioDbContextOptionsMySQLExtensions
{
    public static void UseMysql(
        this ZioDbContextOptions options,
        Action<MySqlDbContextOptionsBuilder>? mysqlOptionsAction = null)
    {
        options.Configure(context => { context.UseMySql(mysqlOptionsAction); });
    }

    public static void UseMysql<TDbContext>(
        this ZioDbContextOptions options,
        Action<MySqlDbContextOptionsBuilder>? mysqlOptionsAction = null)
        where TDbContext : ZioDbContext<TDbContext>
    {
        options.Configure<TDbContext>(context => { context.UseMySql(mysqlOptionsAction); });
    }
}