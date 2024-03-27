using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Zio.Features.Core.DependencyInjection;
using Zio.Features.DI.Autofac.Implementation;

namespace Zio.Features.DI.Autofac.Hosting
{
    public static class ZioAutofacHostBuilderExtensions
    {
        public static IHostBuilder AddAutofac(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((_, services) =>
                {
                    services.Replace(
                        ServiceDescriptor.Scoped<IControllerActivator, ServiceBasedControllerActivator>());
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

                    containerBuilder.Register(c => new CallLoggerInterceptor(Console.Out));

                    containerBuilder.RegisterType<CustomValueObject>();

                    // 获取所有控制器类型并使用属性注入
                    var controllerBaseType = typeof(ControllerBase);
                    containerBuilder.RegisterAssemblyTypes(assembly)
                        .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                        .PropertiesAutowired(new AutowiredPropertySelector());
                });

            return hostBuilder;
        }

        private static void RegisterTypes(ContainerBuilder containerBuilder, IEnumerable<Type> types)
        {
            var generalTypes = types.Where(x =>
                x is { IsAbstract: false, IsInterface: false, IsGenericType: false } &&
                (x.IsAssignableTo<ISingletonDependency>() ||
                 x.IsAssignableTo<IScopedDependency>() ||
                 x.IsAssignableTo<ITransientDependency>()));

            foreach (var type in generalTypes)
            {
                IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>?
                    registrationBuilder = null;

                if (type.IsAssignableTo<ISingletonDependency>())
                {
                    registrationBuilder =
                        containerBuilder
                            .RegisterType(type)
                            .PropertiesAutowired(new AutowiredPropertySelector())
                            .AsImplementedInterfaces()
                            .SingleInstance();
                }

                else if (type.IsAssignableTo<IScopedDependency>())
                {
                    registrationBuilder = containerBuilder
                        .RegisterType(type)
                        .PropertiesAutowired(new AutowiredPropertySelector())
                        .AsImplementedInterfaces()
                        .InstancePerLifetimeScope();
                }

                else if (type.IsAssignableTo<ITransientDependency>())
                {
                    registrationBuilder = containerBuilder
                        .RegisterType(type)
                        .PropertiesAutowired(new AutowiredPropertySelector())
                        .AsImplementedInterfaces()
                        .InstancePerDependency();
                }

                if (registrationBuilder != null && type.GetCustomAttribute<InjectAttribute>() != null)
                {
                    registrationBuilder
                        .Named(type.GetCustomAttribute<InjectAttribute>().Name, type.GetInterfaces().First());
                }

                registrationBuilder?.EnableInterfaceInterceptors();
            }
        }
    }
}