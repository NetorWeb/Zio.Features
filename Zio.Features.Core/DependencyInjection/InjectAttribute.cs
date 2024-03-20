using System;

namespace Zio.Features.Core.DependencyInjection;

[AttributeUsage(AttributeTargets.Class)]
public class InjectAttribute : Attribute
{
    public InjectAttribute()
    {
        
    }

    public string Name { get; set; }
}