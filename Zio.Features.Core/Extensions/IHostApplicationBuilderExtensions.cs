using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Zio.Features.Core.Extensions
{
    public static class IHostApplicationBuilderExtensions
    {
        public static WebApplicationBuilder Inject(this WebApplicationBuilder builder)
        {
            builder.Services.AddHostedService<GenericHostLifetimeEventsHostedService>();

            return builder;
        }
    }
}