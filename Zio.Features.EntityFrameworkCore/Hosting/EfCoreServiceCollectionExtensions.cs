using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zio.Features.Data.Options;
using Zio.Features.EntityFrameworkCore.Options;

namespace Zio.Features.EntityFrameworkCore.Hosting;

public static class EfCoreServiceCollectionExtensions
{
    public static IServiceCollection AddZioDbContext<TDbContext>(this IServiceCollection services, Action<ZioDbContextOptions> action)
        where TDbContext : ZioDbContext<TDbContext>
    {
        services.AddMemoryCache();

        services.TryAddTransient(typeof(ZioDbContext<>));

        var zioDbContextOptions = new ZioDbContextOptions();

        action.Invoke(zioDbContextOptions);

        services.TryAddTransient(DbContextOptionsFactory.Create<TDbContext>);

        return services;
    }
}