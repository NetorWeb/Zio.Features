using Microsoft.Extensions.DependencyInjection;

namespace Zio.Features.EntityFrameworkCore.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddDatabaseAccessor(this IServiceCollection services)
    {

        return services;
    }
}