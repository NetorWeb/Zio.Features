namespace Zio.Features.Guids;

public class ZioSequentialGuidGeneratorOptions
{
    public SequentialGuidType? DefaultSequentialGuidType { get; set; }

    public SequentialGuidType GetDefaultSequentialGuidType()
    {
        return DefaultSequentialGuidType ??
               SequentialGuidType.SequentialAtEnd;
    }
}