﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Zio.Features.Core.DependencyInjection;

namespace Zio.Features.Core.Extensions;

public static class IServiceCollectionExtenison
{
    public static IServiceCollection AddInject(this IServiceCollection services)
    {
        return services;
    }
}