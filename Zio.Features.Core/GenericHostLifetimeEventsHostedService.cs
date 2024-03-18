﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Zio.Features.Core
{
    public class GenericHostLifetimeEventsHostedService   :IHostedService
    {

        public GenericHostLifetimeEventsHostedService(IHost host)
        {
            App.RootServiceProvider = host.Services;
            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}