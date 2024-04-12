using Microsoft.Extensions.Logging;

namespace Zio.Features.Core.Logging;

public interface IExceptionWithSelfLogging
{
    void Log(ILogger logger);
}