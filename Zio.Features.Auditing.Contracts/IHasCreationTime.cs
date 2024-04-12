namespace Zio.Features.Auditing.Contracts;

public interface IHasCreationTime
{
    DateTime CreationTime { get; }
}