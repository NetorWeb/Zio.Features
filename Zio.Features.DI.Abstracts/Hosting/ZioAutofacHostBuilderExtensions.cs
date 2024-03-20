using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zio.Features.Core.DependencyInjection;

namespace Zio.Features.DI.Autofac.Hosting
{
    public static class ZioAutofacHostBuilderExtensions
    {
        public static IHostBuilder AddAutofac(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((_, services) => { })
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

            foreach (var type in types.Where(x =>
                         x is { IsAbstract: false, IsInterface: false } &&
                         (x.IsAssignableTo<ISingletonDependency>() ||
                          x.IsAssignableTo<IScopedDependency>() ||
                          x.IsAssignableTo<ITransientDependency>())))
            {
                IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>?
                    registrationBuilder = null;

                if (type.IsAssignableTo<ISingletonDependency>())
                {
                    registrationBuilder =
                        containerBuilder.RegisterType(type).AsImplementedInterfaces().SingleInstance();
                }

                else if (type.IsAssignableTo<IScopedDependency>())
                {
                    registrationBuilder = containerBuilder.RegisterType(type).AsImplementedInterfaces()
                        .InstancePerLifetimeScope();
                }

                else if (type.IsAssignableTo<ITransientDependency>())
                {
                    registrationBuilder = containerBuilder.RegisterType(type).AsImplementedInterfaces()
                        .InstancePerDependency();
                }

                if (registrationBuilder != null && type.GetCustomAttribute<InjectAttribute>() != null)
                {
                    registrationBuilder.Named(type.GetCustomAttribute<InjectAttribute>().Name, type.GetInterfaces().First());
                }
            }
        }
    }
}