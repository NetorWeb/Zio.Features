using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Zio.Features.Core.DependencyInjection;

namespace Zio.Features.Auditing;

public class SimpleLogAuditingStore : IAuditingStore, ISingletonDependency
{
    public ILogger<SimpleLogAuditingStore> Logger { get; set; }

    public SimpleLogAuditingStore()
    {
        Logger = NullLogger<SimpleLogAuditingStore>.Instance;
    }

    public Task SaveAsync(AuditLogInfo auditLogInfo)
    {
        Logger.LogInformation(auditLogInfo.ToString());
        return Task.FromResult(0);
    }
}