﻿using System.Diagnostics.CodeAnalysis;

namespace Zio.Features.ObjectExtending.ObjectExtending;

public interface IBasicObjectExtensionPropertyInfo
{
    [NotNull]
    public string Name { get; }

    [NotNull]
    public Type Type { get; }

    [NotNull]
    public List<Attribute> Attributes { get; }

    [NotNull]
    public List<Action<ObjectExtensionPropertyValidationContext>> Validators { get; }

    // public ILocalizableString? DisplayName { get; }

    /// <summary>
    /// Uses as the default value if <see cref="DefaultValueFactory"/> was not set.
    /// </summary>
    public object? DefaultValue { get; set; }

    /// <summary>
    /// Used with the first priority to create the default value for the property.
    /// Uses to the <see cref="DefaultValue"/> if this was not set.
    /// </summary>
    public Func<object>? DefaultValueFactory { get; set; }
}