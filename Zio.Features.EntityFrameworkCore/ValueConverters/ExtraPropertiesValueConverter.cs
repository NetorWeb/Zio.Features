﻿using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Zio.Features.Core.Extensions;
using Zio.Features.EntityFrameworkCore.ObjectExtending;
using Zio.Features.Json.SystemTextJson.JsonConverters;
using Zio.Features.ObjectExtending;
using Zio.Features.ObjectExtending.ObjectExtending;

namespace Zio.Features.EntityFrameworkCore.ValueConverters;

public class ExtraPropertiesValueConverter:ValueConverter<ExtraPropertyDictionary, string>
{
    public ExtraPropertiesValueConverter(Type entityType)
        : base(
            d => SerializeObject(d, entityType),
            s => DeserializeObject(s, entityType))
    {

    }
    
    private static string SerializeObject(ExtraPropertyDictionary extraProperties, Type? entityType)
    {
        var copyDictionary = new Dictionary<string, object?>(extraProperties);

        if (entityType != null)
        {
            var objectExtension = ObjectExtensionManager.Instance.GetOrNull(entityType);
            if (objectExtension != null)
            {
                foreach (var property in objectExtension.GetProperties())
                {
                    if (property.IsMappedToFieldForEfCore())
                    {
                        copyDictionary.Remove(property.Name);
                    }
                }
            }
        }

        return JsonSerializer.Serialize(copyDictionary);
    }
    
    public static readonly JsonSerializerOptions DeserializeOptions = new JsonSerializerOptions()
    {
        Converters =
        {
            new ObjectToInferredTypesConverter()
        }
    };
    
    private static ExtraPropertyDictionary DeserializeObject(string extraPropertiesAsJson, Type? entityType)
    {
        if (extraPropertiesAsJson.IsNullOrEmpty() || extraPropertiesAsJson == "{}")
        {
            return new ExtraPropertyDictionary();
        }

        var dictionary = JsonSerializer.Deserialize<ExtraPropertyDictionary>(extraPropertiesAsJson, DeserializeOptions) ??
                         new ExtraPropertyDictionary();

        if (entityType != null)
        {
            var objectExtension = ObjectExtensionManager.Instance.GetOrNull(entityType);
            if (objectExtension != null)
            {
                foreach (var property in objectExtension.GetProperties())
                {
                    dictionary[property.Name] = GetNormalizedValue(dictionary!, property);
                }
            }
        }

        return dictionary;
    }
    
    private static object? GetNormalizedValue(
        Dictionary<string, object> dictionary,
        ObjectExtensionPropertyInfo property)
    {
        var value = dictionary.GetOrDefault(property.Name);
        if (value == null)
        {
            return property.GetDefaultValue();
        }

        try
        {
            if (property.Type.IsEnum)
            {
                return Enum.Parse(property.Type, value.ToString()!, true);
            }

            //return Convert.ChangeType(value, property.Type);
            return value;
        }
        catch
        {
            return value;
        }
    }
}