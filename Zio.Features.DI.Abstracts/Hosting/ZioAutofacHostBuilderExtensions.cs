using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zio.Features.Core.DependencyInjection;


namespace Zio.Features.DI.Autofac.Hosting
{
    public static class ZioAutofacHostBuilderExtensions
    {
        public static IHostBuilder AddAutofac(this IHostBuilder hostBuilder)
        {

            hostBuilder.ConfigureServices((_, services) =>
            {
            })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>((_, containerBuilder) =>
                {
                    var assembly = Assembly.GetEntryAssembly();

                    var types = assembly.GetTypes();

                    RegisterTypes(containerBuilder, types);

                    var assemblies = assembly.GetReferencedAssemblies().Select(Assembly.Load);

                    foreach (var item in assemblies)
                    {
                        RegisterTypes(containerBuilder, item.GetTypes());
                    }
                });

            return hostBuilder;
        }

        private static void RegisterTypes(ContainerBuilder containerBuilder, IEnumerable<Type> types)
        {
            containerBuilder.RegisterTypes(types.Where(t => t.IsAssignableTo<ISingletonDependency>()).ToArray()).AsImplementedInterfaces().SingleInstance();

            containerBuilder.RegisterTypes(types.Where(t => t.IsAssignableTo<IScopedDependency>()).ToArray()).AsImplementedInterfaces().InstancePerLifetimeScope();

            containerBuilder.RegisterTypes(types.Where(t => t.IsAssignableTo<ITransientDependency>()).ToArray()).AsImplementedInterfaces().InstancePerDependency();

        }
    }
}
