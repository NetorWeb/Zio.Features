using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Zio.Features.Core.DependencyInjection;

namespace Zio.Features.Core.Extensions;

public static class IServiceCollectionExtenison
{
    public static IServiceCollection AddInject(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.Scan(scan =>
            {
                scan.FromAssemblies(assembly)
                    .AddClasses(c => c.Where(type => type.IsAssignableTo(typeof(ISingletonDependency))))
                    .AsImplementedInterfaces().WithSingletonLifetime();

                scan.FromAssemblies(assembly)
                    .AddClasses(c => c.Where(type => type.IsAssignableTo(typeof(IScopedDependency))))
                    .AsImplementedInterfaces().WithScopedLifetime();

                scan.FromAssemblies(assembly)
                    .AddClasses(c => c.Where(type => type.IsAssignableTo(typeof(ITransientDependency))))
                    .AsImplementedInterfaces().WithTransientLifetime();
            }
        );

        var referenceAssemblies = assembly.GetReferencedAssemblies().Select(Assembly.Load).ToArray();
        services.Scan(scan =>
            {
                scan.FromAssemblies(referenceAssemblies)
                    .AddClasses(c => c.Where(type => type.IsAssignableTo(typeof(ISingletonDependency))))
                    .AsImplementedInterfaces().WithSingletonLifetime();

                scan.FromAssemblies(referenceAssemblies)
                    .AddClasses(c => c.Where(type => type.IsAssignableTo(typeof(IScopedDependency))))
                    .AsImplementedInterfaces().WithScopedLifetime();

                scan.FromAssemblies(referenceAssemblies)
                    .AddClasses(c => c.Where(type => type.IsAssignableTo(typeof(ITransientDependency))))
                    .AsImplementedInterfaces().WithTransientLifetime();
            }
        );

        return services;
    }
}