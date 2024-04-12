using System;
using System.Diagnostics.CodeAnalysis;
using Zio.Features.Core.Extensions;

namespace Zio.Features.Core;

public static class Check
{
    public static T NotNull<T>(
        T value, 
        string parameterName)
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName);
        }

        return value;
    }

    public static T NotNull<T>(
        T value,
        string parameterName,
        string message)
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName, message);
        }

        return value;
    }
        
    public static string NotNullOrEmpty(
        string? value,
        [NotNull] string parameterName,
        int maxLength = int.MaxValue,
        int minLength = 0)
    {
        if (value.IsNullOrEmpty())
        {
            throw new ArgumentException($"{parameterName} can not be null or empty!", parameterName);
        }

        if (value!.Length > maxLength)
        {
            throw new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
        }

        if (minLength > 0 && value!.Length < minLength)
        {
            throw new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
        }

        return value;
    }
        
    public static string NotNullOrWhiteSpace(
        string? value,
        [NotNull] string parameterName,
        int maxLength = int.MaxValue,
        int minLength = 0)
    {
        if (value.IsNullOrWhiteSpace())
        {
            throw new ArgumentException($"{parameterName} can not be null, empty or white space!", parameterName);
        }

        if (value!.Length > maxLength)
        {
            throw new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
        }

        if (minLength > 0 && value!.Length < minLength)
        {
            throw new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
        }

        return value;
    }
}