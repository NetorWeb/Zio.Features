using Microsoft.Extensions.DependencyInjection;
using Zio.Features.Ddd.Domain.Entities;
using Zio.Features.Ddd.Domain.Options;

namespace Zio.Features.EntityFrameworkCore.Options;

public class ZioDbContextRegistrationOptions : ZioCommonDbContextRegistrationOptions, IZioDbContextRegistrationOptionsBuilder
{

    public Dictionary<Type, object> AbpEntityOptions { get; }

    public ZioDbContextRegistrationOptions(Type originalDbContextType, IServiceCollection services) : base(originalDbContextType, services)
    {
        AbpEntityOptions = new Dictionary<Type, object>();
    }

    public void Entity<TEntity>(Action<ZioEntityOptions<TEntity>> optionsAction) where TEntity : IEntity
    {
        Services.Configure<ZioEntityOptions>(options =>
        {
           options.Entity(optionsAction);
        });
    }
}