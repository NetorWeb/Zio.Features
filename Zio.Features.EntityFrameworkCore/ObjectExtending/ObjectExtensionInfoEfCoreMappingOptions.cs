﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zio.Features.Core;
using Zio.Features.ObjectExtending.ObjectExtending;

namespace Zio.Features.EntityFrameworkCore.ObjectExtending;

public class ObjectExtensionInfoEfCoreMappingOptions
{
    [NotNull]
    public ObjectExtensionInfo ObjectExtension { get; }

    public Action<EntityTypeBuilder>? EntityTypeBuildAction { get; set; }

    public Action<ModelBuilder>? ModelBuildAction { get; set; }

    public ObjectExtensionInfoEfCoreMappingOptions(
        [NotNull] ObjectExtensionInfo objectExtension,
        [NotNull] Action<EntityTypeBuilder> entityTypeBuildAction)
    {
        ObjectExtension = Check.NotNull(objectExtension, nameof(objectExtension));
        EntityTypeBuildAction = Check.NotNull(entityTypeBuildAction, nameof(entityTypeBuildAction));

        EntityTypeBuildAction = entityTypeBuildAction;
    }

    public ObjectExtensionInfoEfCoreMappingOptions(
        [NotNull] ObjectExtensionInfo objectExtension,
        [NotNull] Action<ModelBuilder> modelBuildAction)
    {
        ObjectExtension = Check.NotNull(objectExtension, nameof(objectExtension));
        ModelBuildAction = Check.NotNull(modelBuildAction, nameof(modelBuildAction));

        ModelBuildAction = modelBuildAction;
    }
}