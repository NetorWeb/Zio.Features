using System;

namespace Zio.Features.Core;

[AttributeUsage(AttributeTargets.Class)]
public class InjectAttribute : Attribute
{
    public InjectAttribute()
    {
        
    }

    public string Name { get; set; }
}