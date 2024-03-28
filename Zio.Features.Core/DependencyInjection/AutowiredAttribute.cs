using System;

namespace Zio.Features.Core.DependencyInjection;

/// <summary>
/// 属性注入
/// </summary>
[SuppressSniffer]
[AttributeUsage(AttributeTargets.Property)]
public class AutowiredAttribute : Attribute
{
    
}