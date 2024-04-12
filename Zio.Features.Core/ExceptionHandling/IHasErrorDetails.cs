namespace Zio.Features.Core.ExceptionHandling;

public interface IHasErrorDetails
{
    string? Details { get; }
}