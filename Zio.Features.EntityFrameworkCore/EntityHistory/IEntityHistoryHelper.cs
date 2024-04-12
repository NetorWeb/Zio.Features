using Microsoft.EntityFrameworkCore.ChangeTracking;
using Zio.Features.Auditing;

namespace Zio.Features.EntityFrameworkCore.EntityHistory;

public interface IEntityHistoryHelper
{
    List<EntityChangeInfo> CreateChangeList(ICollection<EntityEntry> entityEntries);

    void UpdateChangeList(List<EntityChangeInfo> entityChanges);
}