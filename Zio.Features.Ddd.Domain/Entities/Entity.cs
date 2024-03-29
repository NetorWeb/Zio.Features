using Zio.Features.Core.Extensions;

namespace Zio.Features.Ddd.Domain.Entities;

[Serializable]
public abstract class Entity : IEntity
{
    public abstract object?[] GetKeys();

    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Keys = {GetKeys().JoinAsString(", ")}";
    }

    public bool EntityEquals(IEntity other)
    {
        return EntityHelper.EntityEquals(this, other);
    }
}

[Serializable]
public abstract class Entity<TKey> : Entity, IEntity<TKey>
{
    public virtual TKey Id { get; protected set; } = default!;

    protected Entity(TKey id)
    {
        Id = id;
    }


}