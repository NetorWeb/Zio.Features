using Zio.Features.Core;

namespace Zio.Features.Auditing.Contracts;

public interface IHasDeletionTime: ISoftDelete
{
    DateTime? DeletionTime { get; }

}