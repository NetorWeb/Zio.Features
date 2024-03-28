using System;
using Autofac;
using Zio.Features.Core;
using Zio.Features.Core.DependencyInjection;

namespace Zio.Features.DI.Autofac.Named;

public class NamedResolver : INamedResolver, ISingletonDependency
{
    public T Get<T>(string serviceName) where T : notnull
    {
        try
        {
            var container = App.GetService<IComponentContext>();

            return container.ResolveNamed<T>(serviceName);
        }
        catch (Exception e)
        {
            throw new NullReferenceException($"无法找到别名为{serviceName}的服务");
        }
    }
}