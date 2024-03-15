using System;
using JetBrains.Annotations;

namespace Zio.Features.Core
{
    public static class Check
    {
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(
            T value, 
            [InvokerParameterName][System.Diagnostics.CodeAnalysis.NotNull] string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(
            T value,
            [InvokerParameterName][System.Diagnostics.CodeAnalysis.NotNull] string parameterName,
            string message)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, message);
            }

            return value;
        }
    }
}
