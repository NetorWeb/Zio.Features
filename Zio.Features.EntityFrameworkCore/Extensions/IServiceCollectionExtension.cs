using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Zio.Features.EntityFrameworkCore.Options;

namespace Zio.Features.EntityFrameworkCore.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddZioDbContext<TDbContext>(
        this IServiceCollection services,
        Action<IZioDbContextRegistrationOptionsBuilder>? optionsBuilder = null)
    where TDbContext : DbContext
    {
        return services;
    }
}