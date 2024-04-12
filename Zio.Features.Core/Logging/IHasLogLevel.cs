using Microsoft.Extensions.Logging;

namespace Zio.Features.Core.Logging;

public interface IHasLogLevel
{
    LogLevel LogLevel { get; set; }
}