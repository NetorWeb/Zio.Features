using System;

namespace Zio.Features.Core;

public class ZioException : Exception
{
    public ZioException()
    {
    }

    public ZioException(string? message)
        : base(message)
    {
    }

    public ZioException(string? message, Exception? innnerException)
        : base(message, innnerException)
    {
    }
}