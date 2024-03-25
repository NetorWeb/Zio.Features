namespace Zio.Features.Service.Test.IServices;

public interface IDelegateInvokeService
{
    object GetData();
}

public interface IHandleService
{
    object GetData(IDictionary<string, object> dic);
}