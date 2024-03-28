using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Zio.Features.Core.Extensions;

namespace Zio.Features.Core.StartUps;

public class StartupFilter : IStartupFilter
{
    /// <summary>
    /// 配置中间件
    /// </summary>
    /// <param name="next"></param>
    /// <returns></returns>
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            // 存储根服务提供器
            App.RootServiceProvider = app.ApplicationServices;

            var envName = App.HostEnvironment?.EnvironmentName ?? "Unknown";

            // 设置响应报文头信息
            app.Use(async (context, next) =>
            {
                // 处理WebSocket请求
                if (context.IsWebSocketRequest()) await next.Invoke();
                else
                {
                    // 输出当前环境标识
                    context.Response.Headers["environment"] = envName;

                    // 执行下一个中间件
                    await next.Invoke();
                }
            });

            app.UseApp();

            // 配置所有 Starup Configure
            UseStartups(app);

            // 调用启动层的 Startups
            next(app);
        };
    }

    /// <summary>
    /// 配置 Startups 的 Configure
    /// </summary>
    /// <param name="app">应用构建器</param>
    private static void UseStartups(IApplicationBuilder app)
    {
        // 反转，处理排序
        var startups = App.AppStartups.Reverse();
        if (!startups.Any()) return;

        UseStartups(startups, app);
    }

    /// <summary>
    /// 批量将自定义 AppStartup 添加到 Startups.cs 的 Configure 中
    /// </summary>
    /// <param name="startups"></param>
    /// <param name="app"></param>
    private static void UseStartups(IEnumerable<AppStartup> startups, IApplicationBuilder app)
    {
        foreach (var startup in startups)
        {
            var type = startup.GetType();

            // 获取所有符合依赖注入格式的方法，如返回值 void，且第一个参数是 IApplicationBuilder 类型
            var configureMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(u => u.ReturnType == typeof(void)
                            && u.GetParameters().Length > 0
                            && u.GetParameters().First().ParameterType == typeof(IApplicationBuilder));

            if (!configureMethods.Any()) continue;

            // 自动安装属性调用
            foreach (var method in configureMethods)
            {
                method.Invoke(startup, ResolveMethodParameterInstances(app, method));
            }
        }

        // 释放内存
        App.AppStartups.Clear();
    }

    /// <summary>
    /// 解析方法参数实例
    /// </summary>
    /// <param name="app"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    private static object[] ResolveMethodParameterInstances(IApplicationBuilder app, MethodInfo method)
    {
        // 获取方法所有参数
        var parameters = method.GetParameters();
        var parameterInstances = new object[parameters.Length];
        parameterInstances[0] = app;

        // 解析服务
        for (var i = 1; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            parameterInstances[i] = app.ApplicationServices.GetRequiredService(parameter.ParameterType);
        }

        return parameterInstances;
    }
}