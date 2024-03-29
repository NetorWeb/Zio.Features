using Zio.Features.Ddd.Domain.Entities;

namespace Zio.Features.Ddd.Domain.Repositories;

public interface IBasicRepository<TEntity>:IReadOnlyBasicRepository<TEntity> where TEntity : class, IEntity
{
    
}