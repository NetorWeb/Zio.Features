namespace Zio.Features.Auditing.Contracts;

public interface IModificationAuditedObject : IHasModificationTime
{
    Guid? LastModifierId { get; }
}

public interface IModificationAuditedObject<TUser> : IModificationAuditedObject
{
    TUser? LastModifier { get; }
}