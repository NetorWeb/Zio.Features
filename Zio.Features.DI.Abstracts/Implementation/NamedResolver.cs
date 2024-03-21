using System;
using Autofac;
using Zio.Features.Core.DependencyInjection;
using Zio.Features.DI.Autofac.Abstraction;

namespace Zio.Features.DI.Autofac.Implementation;

public class NamedResolver(IComponentContext container) : INamedResolver, ISingletonDependency
{
    public T Get<T>(string serviceName) where T : notnull
    {
        try
        {
            return container.ResolveNamed<T>(serviceName);

        }
        catch (Exception e)
        {
            throw new NullReferenceException($"无法找到别名为{serviceName}的服务");
        }
    }
}