using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Hosting;
using Zio.Features.Core.Extensions;
using Zio.Features.Core.HostedServices;
using Zio.Features.Core.StartUps;

namespace Zio.Features.Core;

public static class App
{
    public static IServiceProvider RootServiceProvider;

    public static readonly List<Assembly> Assemblies;

    public static readonly List<Assembly> ExternalAssemblies;

    public static readonly List<Type> EffectiveTypes;

    public static IConfiguration Configuration;

    public static IHostEnvironment HostEnvironment;

    public static IServiceCollection ServiceCollection;


    /// <summary>
    /// 应用所有启动配置对象
    /// </summary>
    internal static ConcurrentBag<AppStartup> AppStartups;

    static App()
    {
        var assObject = GetAssemblies();
        Assemblies = assObject.assemblies.ToList();
        ExternalAssemblies = assObject.externalAssemblies.ToList();

        // 获取有效的类型集合
        EffectiveTypes = Assemblies.SelectMany(GetTypes).ToList();

        AppStartups = new ConcurrentBag<AppStartup>();
    }

    public static T GetService<T>(IServiceProvider? serviceProvider = null)
    {
        T instance;

        if (serviceProvider != null)
        {
            instance = serviceProvider.GetService<T>();
        }

        if (RootServiceProvider == null)
        {
            throw new NullReferenceException();
        }

        var serviceScope = RootServiceProvider.CreateScope();

        instance = serviceScope.ServiceProvider.GetService<T>();

        return instance;
    }

    private static (IEnumerable<Assembly> assemblies, IEnumerable<Assembly> externalAssemblies) GetAssemblies()
    {
        // 需排除的程序集后缀
        var excludeAssemblyNames = new string[]
        {
            "Database.Migrations"
        };

        IEnumerable<Assembly> scanAssemblies;

        // 获取入口程序集
        var entryAssembly = Assembly.GetEntryAssembly();

        // 非独立发布/非单文件发布
        if (!string.IsNullOrWhiteSpace(entryAssembly.Location))
        {
            var dependencyContext = DependencyContext.Default;

            // 读取项目程序集或 Furion 官方发布的包，或手动添加引用的dll，或配置特定的包前缀
            scanAssemblies = dependencyContext.RuntimeLibraries
                .Where(u =>
                    u.Type == "project" && !excludeAssemblyNames.Any(j => u.Name.EndsWith(j))) // 判断是否启用引用程序集扫描
                .Select(u => Reflect.GetAssembly(u.Name));
        }
        else
        {
            // 通过 AppDomain.CurrentDomain 扫描，默认为延迟加载，正常只能扫描到 Furion 和 入口程序集（启动层）
            scanAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(ass =>
                    // 排除 System，Microsoft，netstandard 开头的程序集
                    !ass.FullName.StartsWith(nameof(System))
                    && !ass.FullName.StartsWith(nameof(Microsoft))
                    && !ass.FullName.StartsWith("netstandard"))
                .Distinct();
        }

        IEnumerable<Assembly> externalAssemblies = Array.Empty<Assembly>();

        //TODO 外部程序集？

        return (scanAssemblies, externalAssemblies);
    }

    /// <summary>
    /// 加载程序集中的所有类型
    /// </summary>
    /// <param name="ass"></param>
    /// <returns></returns>
    private static IEnumerable<Type> GetTypes(Assembly ass)
    {
        var types = Array.Empty<Type>();

        try
        {
            types = ass.GetTypes();
        }
        catch
        {
            Console.WriteLine($"Error load `{ass.FullName}` assembly.");
        }

        return types.Where(u => u.IsPublic && !u.IsDefined(typeof(SuppressSnifferAttribute), false));
    }

    /// <summary>
    /// 非 Web
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="autoRegisterBackgroundService"></param>
    internal static void ConfigureApplication(IHostBuilder builder, bool autoRegisterBackgroundService = true)
    {
        // 自动装载配置
        ConfigureHostAppConfiguration(builder);

        // 自动注入 AddApp() 服务
        builder.ConfigureServices((hostContext, services) =>
        {
            // 存储配置对象
            Configuration = hostContext.Configuration;

            // 存储服务提供器
            ServiceCollection = services;

            // 存储根服务
            services.AddHostedService<GenericHostLifetimeEventsHostedService>();

            // 注册 HttpContextAccessor 服务
            services.AddHttpContextAccessor();

            // 注册全局 Startups 扫描
            services.AddStartups();

            // 自动注册 BackgroundService
            if (autoRegisterBackgroundService) services.AddAppHostedService();

            services.AddDefaultDI();

        });
    }

    /// <summary>
    /// Web
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="hostBuilder"></param>
    internal static void ConfigureApplication(IWebHostBuilder builder, IHostBuilder hostBuilder = default)
    {
        // 自动装载配置
        if (hostBuilder == default)
        {
            builder.ConfigureAppConfiguration(((hostContext, configurationBuilder) =>
            {
                // 存储环境对象
                HostEnvironment = hostContext.HostingEnvironment;

                // 加载配置
                AddJsonFiles(configurationBuilder, hostContext.HostingEnvironment);
            }));
        }
        else
        {
            // 自动装载配置
            ConfigureHostAppConfiguration(hostBuilder);
        }

        // 应用初始化服务
        builder.ConfigureServices((hostContext, services) =>
        {
            // 存储配置对象
            Configuration = hostContext.Configuration;

            // 存储根服务器
            ServiceCollection = services;

            // 存储根服务（解决 Web 主机还未启动时在 HostedService 中使用 App.GetService 问题
            services.AddHostedService<GenericHostLifetimeEventsHostedService>();

            // 注册 Startups 过滤器
            services.AddTransient<IStartupFilter, StartupFilter>();

            // 注册 HttpContextAccessor 服务
            services.AddHttpContextAccessor();

            // 注册全局 Startups 扫描
            services.AddStartups();

            services.AddDefaultDI();
        });
    }

    /// <summary>
    /// 自动装载主机配置
    /// </summary>
    /// <param name="builder"></param>
    private static void ConfigureHostAppConfiguration(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((hostContext, configurationBuilder) =>
        {
            // 存储环境对象
            HostEnvironment = hostContext.HostingEnvironment;

            // 加载配置
            AddJsonFiles(configurationBuilder, hostContext.HostingEnvironment);
        });
    }

    internal static void AddJsonFiles(IConfigurationBuilder configurationBuilder, IHostEnvironment hostEnvironment)
    {
        // 获取根配置
#if !NET5_0
        var configuration = configurationBuilder as ConfigurationManager ?? configurationBuilder.Build();
#else
            var configuration = configurationBuilder.Build();
#endif

        // 获取程序执行目录
        var executeDirectory = AppContext.BaseDirectory;

        // 获取自定义配置扫描目录
        var configurationScanDirectories = (configuration.GetSection("ConfigurationScanDirectories")
                                                .Get<string[]>()
                                            ?? Array.Empty<string>())
            .Select(u => Path.Combine(executeDirectory, u));

        // 扫描执行目录及自定义配置目录下的 *.json 文件   
        var jsonFiles = new[] { executeDirectory }
            .Concat(configurationScanDirectories)
            .SelectMany(u =>
                Directory.GetFiles(u, "*.json", SearchOption.TopDirectoryOnly));

        // 如果没有配置文件，中止执行
        if (!jsonFiles.Any()) return;

        // 获取环境变量名，如果没找到，则读取 NETCORE_ENVIRONMENT 环境变量信息识别（用于非 Web 环境）
        var envName = hostEnvironment?.EnvironmentName ??
                      Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT") ?? "Unknown";

        // 读取忽略的配置文件
        var ignoreConfigurationFiles = configuration.GetSection("IgnoreConfigurationFiles")
            .Get<string[]>();
        // 处理控制台应用程序
        var _excludeJsonPrefixs = hostEnvironment == default
            ? excludeJsonPrefixs.Where(u => !u.Equals("appsettings"))
            : excludeJsonPrefixs;

        // 将所有文件进行分组
        var jsonFilesGroups = SplitConfigFileNameToGroups(jsonFiles)
            .Where(u => !_excludeJsonPrefixs.Contains(u.Key, StringComparer.OrdinalIgnoreCase) && !u.Any(c =>
                runtimeJsonSuffixs.Any(z => c.EndsWith(z, StringComparison.OrdinalIgnoreCase)) ||
                ignoreConfigurationFiles.Contains(Path.GetFileName(c), StringComparer.OrdinalIgnoreCase) ||
                ignoreConfigurationFiles.Any(i => new Matcher().AddInclude(i).Match(Path.GetFileName(c)).HasMatches)));

        // 遍历所有配置分组
        foreach (var group in jsonFilesGroups)
        {
            // 限制查找的 json 文件组
            var limitFileNames = new[] { $"{group.Key}.json", $"{group.Key}.{envName}.json" };

            // 查找默认配置和环境配置
            var files = group.Where(u => limitFileNames.Contains(Path.GetFileName(u), StringComparer.OrdinalIgnoreCase))
                .OrderBy(u => Path.GetFileName(u).Length);

            // 循环加载
            foreach (var jsonFile in files)
            {
                configurationBuilder.AddJsonFile(jsonFile, optional: true, reloadOnChange: true);
            }
        }
    }

    /// <summary>
    /// 排除的配置文件前缀
    /// </summary>
    private static readonly string[] excludeJsonPrefixs = new[] { "appsettings", "bundleconfig", "compilerconfig" };

    /// <summary>
    /// 排除运行时 Json 后缀
    /// </summary>
    private static readonly string[] runtimeJsonSuffixs = new[]
    {
        "deps.json",
        "runtimeconfig.dev.json",
        "runtimeconfig.prod.json",
        "runtimeconfig.json",
        "staticwebassets.runtime.json"
    };

    /// <summary>
    /// 对配置文件名进行分组
    /// </summary>
    /// <param name="configFiles"></param>
    /// <returns></returns>
    private static IEnumerable<IGrouping<string, string>> SplitConfigFileNameToGroups(IEnumerable<string> configFiles)
    {
        // 分组
        return configFiles.GroupBy(Function);

        // 本地函数
        static string Function(string file)
        {
            // 根据 . 分隔
            var fileNameParts = Path.GetFileName(file).Split('.', StringSplitOptions.RemoveEmptyEntries);
            if (fileNameParts.Length == 2) return fileNameParts[0];

            return string.Join('.', fileNameParts.Take(fileNameParts.Length - 2));
        }
    }
}