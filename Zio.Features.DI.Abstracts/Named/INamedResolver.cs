namespace Zio.Features.DI.Autofac.Named;

/// <summary>
/// 命名服务解析器
/// </summary>
/// <remarks>
/// 目前只根据实例继承的第一个接口进行服务注册
/// </remarks>
public interface INamedResolver
{
    T Get<T>(string serviceName);
}