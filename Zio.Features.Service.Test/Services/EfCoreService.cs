﻿using Zio.Features.Core.DependencyInjection;
using Zio.Features.Service.Test.IServices;

namespace Zio.Features.Service.Test.Services;

public class EfCoreService: IEfCoreService, ITransientDependency
{
    public EfCoreService(
        )
    {
    }

    public object GetData()
    {
        return "";
    }
}