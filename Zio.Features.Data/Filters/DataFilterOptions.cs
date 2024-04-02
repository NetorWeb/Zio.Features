namespace Zio.Features.Data.Filters;

public class DataFilterOptions
{
    public Dictionary<Type, DataFilterState> DefaultStates { get; } = new();
}