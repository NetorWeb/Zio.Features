namespace Zio.Features.Core.ExceptionHandling;

public interface IHasErrorCode
{
    string? Code { get; }
}