using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zio.Features.Data.Options;

namespace Zio.Features.Data.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ZioDbConnectionOptions>(configuration);

        return services;
    }
}