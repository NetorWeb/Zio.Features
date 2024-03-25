using Zio.Features.Core.DependencyInjection;
using Zio.Features.DI.Autofac.Implementation;
using Zio.Features.Service.Test.IServices;

namespace Zio.Features.Service.Test.Services;

public class DelegateInvokeService : IDelegateInvokeService, ITransientDependency
{
    private readonly IHandleService _handleService;
    private readonly CustomValueObject.Factory _factory;

    public DelegateInvokeService(
        IHandleService handleService,
        CustomValueObject.Factory factory)
    {
        _handleService = handleService;
        _factory = factory;
    }

    public object GetData()
    {
        var obj = _factory.Invoke(new Dictionary<string, object>()
        {
            { "test", "xxxx" },
            { "test2", 222 }
        });

        obj = _factory.Invoke(new Dictionary<string, object>()
        {
            { "test3", "xxxx" },
            { "test4", 6666 }
        });
        return _handleService.GetData(obj.Data);
    }
}

public class HandleService : IHandleService, ITransientDependency
{
    public object GetData(IDictionary<string, object> dic)
    {
        return dic.Select(x => $"{x.Key}={x.Value}");
    }
}