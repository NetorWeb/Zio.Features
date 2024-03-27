using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zio.Features.EntityFrameworkCore.Options;

namespace Zio.Features.EntityFrameworkCore.Hosting;

public static class EfCoreServiceCollectionExtensions
{
    public static IServiceCollection AddZioDbContext<TDbContext>(this IServiceCollection services)
        where TDbContext : ZioDbContext<TDbContext>
    {
        services.AddMemoryCache();

        services.TryAddTransient(DbContextOptionsFactory.Create<TDbContext>);

        return services;
    }
}