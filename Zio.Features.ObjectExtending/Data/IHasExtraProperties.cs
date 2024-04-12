namespace Zio.Features.ObjectExtending.Data;

public interface IHasExtraProperties
{
    ExtraPropertyDictionary ExtraProperties { get; }
}