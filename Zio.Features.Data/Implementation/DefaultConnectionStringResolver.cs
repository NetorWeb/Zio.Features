using Microsoft.Extensions.Options;
using Zio.Features.Core;
using Zio.Features.Core.DependencyInjection;
using Zio.Features.Data.Abstraction;
using Zio.Features.Data.Options;

namespace Zio.Features.Data.Implementation;

public class DefaultConnectionStringResolver : IConnectionStringResolver, ITransientDependency
{

    protected ZioDbConnectionOptions Options { get; }

    public DefaultConnectionStringResolver(
        IOptionsMonitor<ZioDbConnectionOptions> options
        )
    {
        Options = options.CurrentValue;
    }

    public virtual string Resolve(string? connectionStringName = null)
    {
        return ResolveInternal(connectionStringName);
    }

    public virtual Task<string> ResolveAsync(string? connectionStringName = null)
    {
        return Task.FromResult(ResolveInternal(connectionStringName));
    }

    private string? ResolveInternal(string? connectionStringName)
    {
        if (string.IsNullOrEmpty( connectionStringName))
        {
            return Options.ConnectionStrings.Default;
        }

        var connectionString = Options.GetConnectionStringOrNull(connectionStringName);

        if (!string.IsNullOrEmpty(connectionString))
        {
            return connectionString;
        }

        return null;

    }
}