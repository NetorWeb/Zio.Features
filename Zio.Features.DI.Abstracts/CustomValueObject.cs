using System.Collections.Generic;

namespace Zio.Features.DI.Autofac;

/// <summary>
/// 自定义数据实例委托工厂
/// </summary>
/// <remarks>
/// 用字典承载数据
/// </remarks>
public class CustomValueObject(IDictionary<string, object> data)
{
    public delegate CustomValueObject Factory(IDictionary<string, object> data);

    public IDictionary<string,object> Data { get; set; } = data;
}