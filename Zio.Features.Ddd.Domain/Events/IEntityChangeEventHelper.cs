namespace Zio.Features.Ddd.Domain.Events;

public interface IEntityChangeEventHelper
{
    void PublishEntityCreatedEvent(object entity);

    void PublishEntityUpdatedEvent(object entity);

    void PublishEntityDeletedEvent(object entity);
}