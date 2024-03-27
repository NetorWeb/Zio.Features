namespace Zio.Features.Data.Abstraction;

public interface IConnectionStringResolver
{
    string Resolve(string? connectionStringName = null);

    Task<string> ResolveAsync(string? connectionStringName = null);

}