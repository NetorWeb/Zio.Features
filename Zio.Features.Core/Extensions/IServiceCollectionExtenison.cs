using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Zio.Features.Core.DependencyInjection;
using Zio.Features.Core.StartUps;

namespace Zio.Features.Core.Extensions;

public static class IServiceCollectionExtenison
{
    public static IServiceCollection AddInject(this IServiceCollection services)
    {
        return services;
    }

    /// <summary>
    /// 添加 Startups 自动扫描
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    internal static IServiceCollection AddStartups(this IServiceCollection services)
    {
        // 扫描所有继承 AppStartup 的类
        var startups = App.EffectiveTypes
            .Where(u => typeof(AppStartup).IsAssignableFrom(u) &&
                        u is { IsClass: true, IsAbstract: false, IsGenericType: false })
            .OrderByDescending(u => GetStartupOrder(u));

        // 注册自定义 startup
        foreach (var type in startups)
        {
            var startup = Activator.CreateInstance(type) as AppStartup;

            App.AppStartups.Add(startup);

            // 获取所有符合依赖注入格式的方法，如返回值void，且第一个参数是 IServiceCollection 类型
            var serviceMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(u => u.ReturnType == typeof(void)
                            && u.GetParameters().Length > 0
                            && u.GetParameters().First().ParameterType == typeof(IServiceCollection));

            if (!serviceMethods.Any()) continue;

            // 自动安装属性调用
            foreach (var method in serviceMethods)
            {
                method.Invoke(startup, new[] { services });
            }
        }

        return services;
    }

    /// <summary>
    /// 自动添加主机服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddAppHostedService(this IServiceCollection services)
    {
        // 获取所有 BackgroundService 类型，排除泛型主机
        var backgroundServiceTypes = App.EffectiveTypes.Where(u =>
            typeof(IHostedService).IsAssignableFrom(u) && u.Name != "GenericWebHostService");
        var addHostServiceMethod = typeof(ServiceCollectionHostedServiceExtensions)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(u => u.Name.Equals("AddHostedService") && u.IsGenericMethod && u.GetParameters().Length == 1)
            .FirstOrDefault();

        foreach (var type in backgroundServiceTypes)
        {
            addHostServiceMethod.MakeGenericMethod(type).Invoke(null, new object[] { services });
        }

        return services;
    }

    /// <summary>
    /// 获取 Startups 排序
    /// </summary>
    /// <param name="type">排序类型</param>
    /// <returns>int</returns>
    private static int GetStartupOrder(Type type)
    {
        return !type.IsDefined(typeof(AppStartupAttribute), true)
            ? 0
            : type.GetCustomAttribute<AppStartupAttribute>(true).Order;
    }

    public static IServiceCollection AddDefaultDI(this IServiceCollection services)
    {
        RegisterType(services, App.EffectiveTypes);
        return services;
    }

    private static void RegisterType(IServiceCollection services, List<Type> types)
    {
        foreach (var type in types
                     .Where(x => x is { IsInterface: false, IsAbstract: false } &&
                                 x.IsAssignableTo(typeof(ISingletonDependency))))
        {
            var interfaces = type.GetInterfaces().Where(x => !x.IsAssignableTo(typeof(ISingletonDependency)))
                .ToArray();
            if (interfaces.Length != 0)
            {
                services.TryAddSingleton(interfaces.First(), type);
            }
            else
            {
                services.TryAddSingleton(type);
            }
        }

        foreach (var type in types
                     .Where(x => x is { IsInterface: false, IsAbstract: false } &&
                                 x.IsAssignableTo(typeof(IScopedDependency))))
        {
            var interfaces = type.GetInterfaces().Where(x => !x.IsAssignableTo(typeof(IScopedDependency)))
                .ToArray();
            if (interfaces.Length != 0)
            {
                services.TryAddScoped(interfaces.First(), type);
            }
            else
            {
                services.TryAddScoped(type);
                services.TryAddScoped(type);
            }
        }

        foreach (var type in types
                     .Where(x => x is { IsInterface: false, IsAbstract: false } &&
                                 x.IsAssignableTo(typeof(ITransientDependency))))
        {
            var interfaces = type.GetInterfaces().Where(x => !x.IsAssignableTo(typeof(ITransientDependency)))
                .ToArray();
            if (interfaces.Length != 0)
            {
                services.TryAddTransient(interfaces.First(), type);
            }
            else
            {
                services.TryAddTransient(type);
            }
        }
    }
}