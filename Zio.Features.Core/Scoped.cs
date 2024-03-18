using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Zio.Features.Core
{
    public static class Scoped
    {
        public static void Create(Action<IServiceScopeFactory?, IServiceScope> handler, IServiceScopeFactory? serviceScopeFactory = null)
        {
            
            using var serviceScope = App.RootServiceProvider.CreateScope();

             serviceScopeFactory ??= App.RootServiceProvider.GetService<IServiceScopeFactory>();

            handler(serviceScopeFactory, serviceScope);
        }

        public static async Task CreateAsync(Action<IServiceScopeFactory?, IServiceScope> handler, IServiceScopeFactory? serviceScopeFactory = null)
        {
            await using var serviceScope = App.RootServiceProvider.CreateAsyncScope();
            
             serviceScopeFactory ??= App.RootServiceProvider.GetService<IServiceScopeFactory>();

            handler(serviceScopeFactory, serviceScope);
        }
    }
}