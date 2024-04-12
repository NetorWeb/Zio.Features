using System;
using System.Collections.Generic;
using System.Linq;

namespace Zio.Features.Core.Reflection;

public static class TypeHelper
{
    private static readonly HashSet<Type> NonNullablePrimitiveTypes = new HashSet<Type>
    {
        typeof(byte),
        typeof(short),
        typeof(int),
        typeof(long),
        typeof(sbyte),
        typeof(ushort),
        typeof(uint),
        typeof(ulong),
        typeof(bool),
        typeof(float),
        typeof(decimal),
        typeof(DateTime),
        typeof(DateTimeOffset),
        typeof(TimeSpan),
        typeof(Guid)
    };

    public static object? GetDefaultValue(Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }

        return null;
    }

    public static bool IsDefaultValue(object? obj)
    {
        if (obj == null)
        {
            return true;
        }

        return obj.Equals(GetDefaultValue(obj.GetType()));
    }
    
    public static bool IsPrimitiveExtended(Type type, bool includeNullables = true, bool includeEnums = false)
    {
        if (IsPrimitiveExtendedInternal(type, includeEnums))
        {
            return true;
        }

        if (includeNullables && IsNullable(type) && type.GenericTypeArguments.Any())
        {
            return IsPrimitiveExtendedInternal(type.GenericTypeArguments[0], includeEnums);
        }

        return false;
    }
    
    public static bool IsNullable(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
    
    public static Type GetFirstGenericArgumentIfNullable(this Type t)
    {
        if (t.GetGenericArguments().Length > 0 && t.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return t.GetGenericArguments().First();
        }

        return t;
    }
    
    public static bool IsNonNullablePrimitiveType(Type type)
    {
        return NonNullablePrimitiveTypes.Contains(type);
    }
    
    private static bool IsPrimitiveExtendedInternal(Type type, bool includeEnums)
    {
        if (type.IsPrimitive)
        {
            return true;
        }

        if (includeEnums && type.IsEnum)
        {
            return true;
        }

        return type == typeof(string) ||
               type == typeof(decimal) ||
               type == typeof(DateTime) ||
               type == typeof(DateTimeOffset) ||
               type == typeof(TimeSpan) ||
               type == typeof(Guid);
    }
}