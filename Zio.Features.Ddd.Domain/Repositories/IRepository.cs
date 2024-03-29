namespace Zio.Features.Ddd.Domain.Repositories;

public interface IRepository
{
    bool? IsChangeTrackingEnabled { get; }
}

