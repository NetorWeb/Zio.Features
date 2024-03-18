using System;
using Microsoft.Extensions.DependencyInjection;

namespace Zio.Features.Core
{
    public static class App
    {
        public static IServiceProvider RootServiceProvider { get; set; }

        public static T GetService<T>(IServiceProvider? serviceProvider)
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
    }
}