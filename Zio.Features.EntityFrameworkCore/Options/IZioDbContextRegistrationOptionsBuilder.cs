using System.Diagnostics.CodeAnalysis;
using Zio.Features.Ddd.Domain.Entities;
using Zio.Features.Ddd.Domain.Options;

namespace Zio.Features.EntityFrameworkCore.Options;

public interface IZioDbContextRegistrationOptionsBuilder : IZioCommonDbContextRegistrationOptionsBuilder
{
    void Entity<TEntity>([NotNull] Action<ZioEntityOptions<TEntity>> optionsAction)
        where TEntity : IEntity;
}