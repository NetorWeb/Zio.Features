using Microsoft.EntityFrameworkCore.ChangeTracking;
using Zio.Features.Auditing;

namespace Zio.Features.EntityFrameworkCore.EntityHistory;

public class NullEntityHistoryHelper: IEntityHistoryHelper
{
    public static NullEntityHistoryHelper Instance { get; } = new NullEntityHistoryHelper();

    public NullEntityHistoryHelper()
    {
        
    }
    
    public List<EntityChangeInfo> CreateChangeList(ICollection<EntityEntry> entityEntries)
    {
        return new List<EntityChangeInfo>();
    }

    public void UpdateChangeList(List<EntityChangeInfo> entityChanges)
    {
        
    }
}