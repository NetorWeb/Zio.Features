namespace Zio.Features.Auditing.Contracts;

public interface IHasModificationTime
{
    DateTime? LastModificationTime { get; }
}