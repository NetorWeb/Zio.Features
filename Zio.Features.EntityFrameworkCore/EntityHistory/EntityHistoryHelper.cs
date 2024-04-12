using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Zio.Features.Auditing;
using Zio.Features.Core.DependencyInjection;

namespace Zio.Features.EntityFrameworkCore.EntityHistory;


//Todo 空缺

public class EntityHistoryHelper : IEntityHistoryHelper, ITransientDependency
{
    public ILogger<EntityHistoryHelper> Logger { get; set; }
    
    protected IAuditingStore AuditingStore { get; }

    //protected IJsonSerializer JsonSerializer { get; }
    
    public List<EntityChangeInfo> CreateChangeList(ICollection<EntityEntry> entityEntries)
    {
        throw new NotImplementedException();
    }

    public void UpdateChangeList(List<EntityChangeInfo> entityChanges)
    {
        throw new NotImplementedException();
    }
}