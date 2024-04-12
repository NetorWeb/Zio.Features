using System;

namespace Zio.Features.Core.Timing;

public class ZioClockOptions
{
    /// <summary>
    /// Default: <see cref="DateTimeKind.Unspecified"/>
    /// </summary>
    public DateTimeKind Kind { get; set; }

    public ZioClockOptions()
    {
        Kind = DateTimeKind.Unspecified;
    }
}