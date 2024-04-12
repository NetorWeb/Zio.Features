namespace Zio.Features.Auditing.Contracts;

public interface IHasEntityVersion
{
    int EntityVersion { get; }
}