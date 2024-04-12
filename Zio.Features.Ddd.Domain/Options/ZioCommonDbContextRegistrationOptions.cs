using Microsoft.Extensions.DependencyInjection;
using Zio.Features.Core;
using Zio.Features.Core.Extensions;
using Zio.Features.Ddd.Domain.Entities;
using Zio.Features.Ddd.Domain.Repositories;

namespace Zio.Features.Ddd.Domain.Options;

public abstract class ZioCommonDbContextRegistrationOptions: IZioCommonDbContextRegistrationOptionsBuilder
{
    public IServiceCollection Services { get; }

    public Type OriginalDbContextType { get; }

    public Type DefaultRepositoryDbContextType { get; protected set; }

    public Type? DefaultRepositoryImplementationType { get; private set; }

    public Type? DefaultRepositoryImplementationTypeWithoutKey { get; private set; }

    public bool RegisterDefaultRepositories { get; private set; }

    public bool IncludeAllEntitiesForDefaultRepositories { get; private set; }

    public Dictionary<Type, Type> CustomRepositories { get; }

    public List<Type> SpecifiedDefaultRepositories { get; }

    public bool SpecifiedDefaultRepositoryTypes => DefaultRepositoryImplementationType != null && DefaultRepositoryImplementationTypeWithoutKey != null;

    protected ZioCommonDbContextRegistrationOptions(
        Type originalDbContextType,
        IServiceCollection services
        )
    {
        OriginalDbContextType = originalDbContextType;
        Services = services;
        DefaultRepositoryDbContextType = originalDbContextType;
        CustomRepositories = new Dictionary<Type, Type>();
        SpecifiedDefaultRepositories = new List<Type>();
    }

    public IZioCommonDbContextRegistrationOptionsBuilder AddDefaultRepositories(bool includeAllEntities = false)
    {
        RegisterDefaultRepositories = true;
        IncludeAllEntitiesForDefaultRepositories = includeAllEntities;

        return this;
    }

    public IZioCommonDbContextRegistrationOptionsBuilder AddDefaultRepositories<TDefaultRepositoryDbContext>(
        bool includeAllEntities = false)
    {
        return AddDefaultRepositories(typeof(TDefaultRepositoryDbContext), includeAllEntities);
    }

    public IZioCommonDbContextRegistrationOptionsBuilder AddDefaultRepositories(Type defaultRepositoryDbContextType,
        bool includeAllEntities = false)
    {
        if (!defaultRepositoryDbContextType.IsAssignableFrom(OriginalDbContextType))
        {
            throw new Exception($"{OriginalDbContextType.AssemblyQualifiedName} should inherit/implement {{defaultRepositoryDbContextType.AssemblyQualifiedName}}!");
        }

        DefaultRepositoryImplementationType = defaultRepositoryDbContextType;

        return AddDefaultRepositories(includeAllEntities);
    }

    public IZioCommonDbContextRegistrationOptionsBuilder AddDefaultRepository<TEntity>()
    {
        return AddDefaultRepository(typeof(TEntity));
    }

    public IZioCommonDbContextRegistrationOptionsBuilder AddDefaultRepository(Type entityType)
    {
        EntityHelper.CheckEntity(entityType);

        SpecifiedDefaultRepositories.AddIfNotContains(entityType);

        return this;
    }

    public IZioCommonDbContextRegistrationOptionsBuilder AddRepository<TEntity, TRepository>()
    {
        AddCustomRepository(typeof(TEntity), typeof(TRepository));

        return this;
    }

    public IZioCommonDbContextRegistrationOptionsBuilder SetDefaultRepositoryClasses(Type repositoryImplementationType,
        Type repositoryImplementationTypeWithoutKey)
    {
        Check.NotNull(repositoryImplementationType, nameof(repositoryImplementationType));
        Check.NotNull(repositoryImplementationTypeWithoutKey, nameof(repositoryImplementationTypeWithoutKey));

        DefaultRepositoryImplementationType = repositoryImplementationType;
        DefaultRepositoryImplementationTypeWithoutKey = repositoryImplementationTypeWithoutKey;

        return this;
    }

    private void AddCustomRepository(Type entityType, Type repositoryType)
    {
        if (!typeof(IEntity).IsAssignableFrom(entityType))
        {
            throw new Exception($"Given entityType is not an entity: {entityType.AssemblyQualifiedName}. It must implement {typeof(IEntity<>).AssemblyQualifiedName}.");
        }

        if (!typeof(IRepository).IsAssignableFrom(repositoryType))
        {
            throw new Exception($"Given repositoryType is not a repository: {entityType.AssemblyQualifiedName}. It must implement {typeof(IBasicRepository<>).AssemblyQualifiedName}.");
        }

        CustomRepositories[entityType] = repositoryType;
    }
}