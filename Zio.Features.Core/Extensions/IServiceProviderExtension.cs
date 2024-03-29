using System;
using Microsoft.Extensions.DependencyInjection;

namespace Zio.Features.Core.Extensions;

public static class IServiceProviderExtension
{
    public static object? GetKeyedService(this IServiceProvider provider, Type serviceType, object? serviceKey)
    {
        Check.NotNull(provider, nameof(provider));

        if (provider is IKeyedServiceProvider keyedServiceProvider)
        {
            return keyedServiceProvider.GetKeyedService(serviceType, serviceKey);
        }

        throw new InvalidOperationException("This service provider doesn't support keyed services.");
    }
}