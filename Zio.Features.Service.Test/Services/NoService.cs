using Zio.Features.Core.DependencyInjection;

namespace Zio.Features.Service.Test.Services;

public class NoService : ITransientDependency
{
    public object GetData()
    {
        return "";
    }
}