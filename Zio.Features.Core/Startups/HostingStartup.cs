using Microsoft.AspNetCore.Hosting;
using Zio.Features.Core.StartUps;

[assembly: HostingStartup(typeof(HostingStartup))]

namespace Zio.Features.Core.StartUps;

public class HostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        App.ConfigureApplication(builder);
    }
}