﻿using System.Diagnostics.CodeAnalysis;
using Zio.Features.Core;
using Zio.Features.Core.Reflection;
using Zio.Features.ObjectExtending.ObjectExtending.Modularity;

namespace Zio.Features.ObjectExtending.ObjectExtending;

public class ObjectExtensionPropertyInfo:IBasicObjectExtensionPropertyInfo
{
    [NotNull]
    public ObjectExtensionInfo ObjectExtension { get; }
    
    [NotNull]
    public string Name { get; }
    
    [NotNull]
    public Type Type { get; }
    
    [NotNull]
    public List<Attribute> Attributes { get; }
    
    [NotNull]
    public List<Action<ObjectExtensionPropertyValidationContext>> Validators { get; }
    
    /// <summary>
    /// Indicates whether to check the other side of the object mapping
    /// if it explicitly defines the property. This property is used in;
    ///
    /// * .MapExtraPropertiesTo() extension method.
    /// * .MapExtraProperties() configuration for the AutoMapper.
    ///
    /// It this is true, these methods check if the mapping object
    /// has defined the property using the <see cref="ObjectExtensionManager"/>.
    ///
    /// Default: null (unspecified, uses the default logic).
    /// </summary>
    public bool? CheckPairDefinitionOnMapping { get; set; }

    [NotNull]
    public Dictionary<object, object> Configuration { get; }
    
    public object? DefaultValue { get; set; }
    public Func<object>? DefaultValueFactory { get; set; }
    
    public ExtensionPropertyLookupConfiguration Lookup { get; set; }
    
    public ExtensionPropertyUI UI { get; set; }

    public ObjectExtensionPropertyInfo(
        [NotNull] ObjectExtensionInfo objectExtension,
        [NotNull] Type type,
        [NotNull] string name)
    {
        ObjectExtension = Check.NotNull(objectExtension, nameof(objectExtension));
        Type = Check.NotNull(type, nameof(type));
        Name = Check.NotNull(name, nameof(name));

        Configuration = new Dictionary<object, object>();
        Attributes = new List<Attribute>();
        Validators = new List<Action<ObjectExtensionPropertyValidationContext>>();

        Attributes.AddRange(ExtensionPropertyHelper.GetDefaultAttributes(Type));
        DefaultValue = TypeHelper.GetDefaultValue(Type);
        Lookup = new ExtensionPropertyLookupConfiguration();
        UI = new ExtensionPropertyUI();
    }
    
    public object? GetDefaultValue()
    {
        return ExtensionPropertyHelper.GetDefaultValue(Type, DefaultValueFactory, DefaultValue);
    }
    
    public class ExtensionPropertyUI
    {
        public int Order { get; set; }
        
        public ExtensionPropertyUIEditModal EditModal { get; set; }

        public ExtensionPropertyUI()
        {
            EditModal = new ExtensionPropertyUIEditModal();
        }
    }

    public class ExtensionPropertyUIEditModal
    {
        public bool IsReadOnly { get; set; }
    }

}