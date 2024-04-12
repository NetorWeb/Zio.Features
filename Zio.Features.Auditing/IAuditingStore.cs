namespace Zio.Features.Auditing;

public interface IAuditingStore
{
    Task SaveAsync(AuditLogInfo auditLogInfo);
}