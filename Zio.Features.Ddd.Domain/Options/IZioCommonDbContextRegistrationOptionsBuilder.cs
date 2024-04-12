using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Zio.Features.Ddd.Domain.Options;

public interface IZioCommonDbContextRegistrationOptionsBuilder
{
    IServiceCollection Services { get; }

    IZioCommonDbContextRegistrationOptionsBuilder AddDefaultRepositories(bool includeAllEntities = false);

    IZioCommonDbContextRegistrationOptionsBuilder AddDefaultRepositories<TDefaultRepositoryDbContext>(bool includeAllEntities = false);

    IZioCommonDbContextRegistrationOptionsBuilder AddDefaultRepositories(Type defaultRepositoryDbContextType, bool includeAllEntities = false);

    IZioCommonDbContextRegistrationOptionsBuilder AddDefaultRepository<TEntity>();

    IZioCommonDbContextRegistrationOptionsBuilder AddDefaultRepository(Type entityType);

    IZioCommonDbContextRegistrationOptionsBuilder AddRepository<TEntity, TRepository>();

    IZioCommonDbContextRegistrationOptionsBuilder SetDefaultRepositoryClasses([NotNull] Type repositoryImplementationType, [NotNull] Type repositoryImplementationTypeWithoutKey);

}