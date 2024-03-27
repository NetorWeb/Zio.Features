using Zio.Features.Core;
using Zio.Features.Core.Abstraction.DependencyInjection;
using Zio.Features.Service.Test.IServices;

namespace Zio.Features.Service.Test.Services;

[Inject(Name = "Named1")]
public class Named1Service: INamedService, ITransientDependency
{
    public string GetName()
    {
        return nameof(Named1Service);
    }
}

[Inject(Name = "Named2")]
public class Named2Service : INamedService, ITransientDependency
{
    public string GetName()
    {
        return nameof(Named2Service);
    }
}