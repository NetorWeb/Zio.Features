using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zio.Features.Core;
using Zio.Features.Core.Extensions;
using Zio.Features.ObjectExtending.ObjectExtending;

namespace Zio.Features.EntityFrameworkCore.ObjectExtending;

public static class EfCoreObjectExtensionPropertyInfoExtensions
{
    public const string EfCorePropertyConfigurationName = "EfCoreMapping";

    public static ObjectExtensionPropertyInfo MapEfCore(
        [NotNull] this ObjectExtensionPropertyInfo propertyExtension)
    {
        Check.NotNull(propertyExtension, nameof(propertyExtension));

        propertyExtension.Configuration[EfCorePropertyConfigurationName] =
            new ObjectExtensionPropertyInfoEfCoreMappingOptions(
                propertyExtension
            );

        return propertyExtension;
    }

    [Obsolete("Use MapEfCore with EntityTypeAndPropertyBuildAction parameters.")]
    public static ObjectExtensionPropertyInfo MapEfCore(
        [NotNull] this ObjectExtensionPropertyInfo propertyExtension,
        Action<PropertyBuilder> propertyBuildAction)
    {
        Check.NotNull(propertyExtension, nameof(propertyExtension));

        propertyExtension.Configuration[EfCorePropertyConfigurationName] =
            new ObjectExtensionPropertyInfoEfCoreMappingOptions(
                propertyExtension,
                propertyBuildAction
            );

        return propertyExtension;
    }

    public static ObjectExtensionPropertyInfo MapEfCore(
        [NotNull] this ObjectExtensionPropertyInfo propertyExtension,
        Action<EntityTypeBuilder, PropertyBuilder>? entityTypeAndPropertyBuildAction)
    {
        Check.NotNull(propertyExtension, nameof(propertyExtension));

        propertyExtension.Configuration[EfCorePropertyConfigurationName] =
            new ObjectExtensionPropertyInfoEfCoreMappingOptions(
                propertyExtension,
                entityTypeAndPropertyBuildAction
            );

        return propertyExtension;
    }

    public static ObjectExtensionPropertyInfoEfCoreMappingOptions? GetEfCoreMappingOrNull(
        [NotNull] this ObjectExtensionPropertyInfo propertyExtension)
    {
        Check.NotNull(propertyExtension, nameof(propertyExtension));

        return propertyExtension
                .Configuration
                .GetOrDefault(EfCorePropertyConfigurationName)
            as ObjectExtensionPropertyInfoEfCoreMappingOptions;
    }
    
    public static bool IsMappedToFieldForEfCore(
        [NotNull] this ObjectExtensionPropertyInfo propertyExtension)
    {
        Check.NotNull(propertyExtension, nameof(propertyExtension));

        return propertyExtension
            .Configuration
            .ContainsKey(EfCorePropertyConfigurationName);
    }
}