namespace Zio.Features.Ddd.Domain.Events;

public class NullEntityChangeEventHelper:IEntityChangeEventHelper
{

    /// <summary>
    /// Gets single instance of <see cref="NullEntityChangeEventHelper"/> class.
    /// </summary>
    public static NullEntityChangeEventHelper Instance { get; } = new NullEntityChangeEventHelper();

    public NullEntityChangeEventHelper()
    {
        
    }

    public void PublishEntityCreatedEvent(object entity)
    {
    }

    public void PublishEntityUpdatedEvent(object entity)
    {
    }

    public void PublishEntityDeletedEvent(object entity)
    {
    }
}