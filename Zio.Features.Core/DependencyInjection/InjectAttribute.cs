using System;

namespace Zio.Features.Core.DependencyInjection;

/// <summary>
/// 别名服务
/// </summary>
[SuppressSniffer]
[AttributeUsage(AttributeTargets.Class)]
public class InjectAttribute : Attribute
{
    public InjectAttribute()
    {
        
    }

    public string Name { get; set; }
}