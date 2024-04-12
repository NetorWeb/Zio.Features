namespace Zio.Features.Data.Entities;

public interface IHasConcurrencyStamp
{
    string ConcurrencyStamp { get; set; }
}