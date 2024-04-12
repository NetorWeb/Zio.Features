using Zio.Features.Auditing.Contracts;
using Zio.Features.Core;
using Zio.Features.Core.DependencyInjection;
using Zio.Features.Core.Timing;
using Zio.Features.Security.Users;

namespace Zio.Features.Auditing;

public class AuditPropertySetter : IAuditPropertySetter, ITransientDependency
{
    protected ICurrentUser CurrentUser { get; }

    protected IClock Clock { get; }

    public AuditPropertySetter(
        ICurrentUser currentUser,
        IClock clock
    )
    {
        CurrentUser = currentUser;
        Clock = clock;
    }

    public void SetCreationProperties(object targetObject)
    {
        SetCreationTime(targetObject);
        SetCreatorId(targetObject);
    }

    public void SetModificationProperties(object targetObject)
    {
        SetLastModificationTime(targetObject);
        SetLastModifierId(targetObject);
    }

    public void SetDeletionProperties(object targetObject)
    {
        SetDeletionTime(targetObject);
        SetDeleterId(targetObject);
    }

    public void IncrementEntityVersionProperty(object targetObject)
    {
        if (targetObject is IHasEntityVersion objectWithEntityVersion)
        {
            ObjectHelper.TrySetProperty(objectWithEntityVersion, x => x.EntityVersion, x => x.EntityVersion + 1);
        }
    }

    protected virtual void SetCreationTime(object targetObject)
    {
        if (targetObject is not IHasCreationTime objectWithCreationTime)
        {
            return;
        }

        if (objectWithCreationTime.CreationTime == default)
        {
            ObjectHelper.TrySetProperty(objectWithCreationTime, x => x.CreationTime, () => Clock.Now);
        }
    }

    protected virtual void SetCreatorId(object targetObject)
    {
        if (!CurrentUser.Id.HasValue)
        {
            return;
        }

        if (targetObject is IMayHaveCreator mayHaveCreatorObject)
        {
            if (mayHaveCreatorObject.CreatorId.HasValue && mayHaveCreatorObject.CreatorId.Value != default)
            {
                return;
            }

            ObjectHelper.TrySetProperty(mayHaveCreatorObject, x => x.CreatorId, () => CurrentUser.Id);
        }
        else if (targetObject is IMustHaveCreator mustHaveCreatorObject)
        {
            if (mustHaveCreatorObject.CreatorId != default)
            {
                return;
            }

            ObjectHelper.TrySetProperty(mustHaveCreatorObject, x => x.CreatorId, () => CurrentUser.Id.Value);
        }
    }
    
    protected virtual void SetLastModificationTime(object targetObject)
    {
        if (targetObject is IHasModificationTime objectWithModificationTime)
        {
            ObjectHelper.TrySetProperty(objectWithModificationTime, x => x.LastModificationTime, () => Clock.Now);
        }
    }
    
    protected virtual void SetLastModifierId(object targetObject)
    {
        if (!(targetObject is IModificationAuditedObject modificationAuditedObject))
        {
            return;
        }

        if (!CurrentUser.Id.HasValue)
        {
            ObjectHelper.TrySetProperty(modificationAuditedObject, x => x.LastModifierId, () => null);
            return;
        }

        ObjectHelper.TrySetProperty(modificationAuditedObject, x => x.LastModifierId, () => CurrentUser.Id);
    }
    
    protected virtual void SetDeletionTime(object targetObject)
    {
        if (targetObject is IHasDeletionTime objectWithDeletionTime)
        {
            if (objectWithDeletionTime.DeletionTime == null)
            {
                ObjectHelper.TrySetProperty(objectWithDeletionTime, x => x.DeletionTime, () => Clock.Now);
            }
        }
    }

    protected virtual void SetDeleterId(object targetObject)
    {
        if (!(targetObject is IDeletionAuditedObject deletionAuditedObject))
        {
            return;
        }

        if (deletionAuditedObject.DeleterId != null)
        {
            return;
        }

        if (!CurrentUser.Id.HasValue)
        {
            ObjectHelper.TrySetProperty(deletionAuditedObject, x => x.DeleterId, () => null);
            return;
        }

        ObjectHelper.TrySetProperty(deletionAuditedObject, x => x.DeleterId, () => CurrentUser.Id);
    }
}