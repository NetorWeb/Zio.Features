using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zio.Features.Core.DependencyInjection;

namespace Zio.Features.Core.Extensions
{
    public static class IHostApplicationBuilderExtensions
    {
        public static WebApplicationBuilder Inject(this WebApplicationBuilder builder)
        {
            App.ConfigureApplication(builder.Host);

            return builder;
        }

        /// <summary>
        /// 添加应用中间件
        /// </summary>
        /// <param name="app">应用构建器</param>
        /// <param name="configure">应用配置</param>
        /// <returns>应用构建器</returns>
        internal static IApplicationBuilder UseApp(this IApplicationBuilder app,
            Action<IApplicationBuilder> configure = null)
        {
            // 调用自定义服务
            configure?.Invoke(app);
            return app;
        }

        
    }
}