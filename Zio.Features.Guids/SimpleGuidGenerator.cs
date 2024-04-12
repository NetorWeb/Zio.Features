namespace Zio.Features.Guids;

public class SimpleGuidGenerator : IGuidGenerator
{

    public static SimpleGuidGenerator Instance { get; } = new SimpleGuidGenerator();

    public Guid Create()
    {
        return Guid.NewGuid();
    }
}