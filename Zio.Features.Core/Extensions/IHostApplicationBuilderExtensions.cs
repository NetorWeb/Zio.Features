using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zio.Features.Core.DependencyInjection;

namespace Zio.Features.Core.Extensions
{
    public static class IHostApplicationBuilderExtensions
    {
        public static WebApplicationBuilder Inject(this WebApplicationBuilder builder)
        {
            builder.Services.AddHostedService<GenericHostLifetimeEventsHostedService>();

            var assembly = Assembly.GetExecutingAssembly();
            builder.Services.Scan(scan =>
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
            builder.Services.Scan(scan =>
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

            return builder;
        }
    }
}