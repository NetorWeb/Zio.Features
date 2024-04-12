using System;

namespace Zio.Features.Core.DependencyInjection;

public interface IServiceProviderAccessor
{
    IServiceProvider ServiceProvider { get; }
}