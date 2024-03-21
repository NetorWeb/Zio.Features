using System.Linq;
using System.Reflection;
using Autofac.Core;
using Zio.Features.Core.DependencyInjection;

namespace Zio.Features.DI.Autofac;

public class AutowiredPropertySelector : IPropertySelector
{
    public bool InjectProperty(PropertyInfo propertyInfo, object instance)
    {
        return propertyInfo.CustomAttributes.Any(x => x.AttributeType == typeof(AutowiredAttribute));
    }
}