using System;
using Microsoft.Extensions.Options;
using Zio.Features.Core.DependencyInjection;

namespace Zio.Features.Core.Timing;

public class Clock : IClock, ITransientDependency
{
    protected ZioClockOptions Options { get; }

    public Clock(IOptions<ZioClockOptions> options)
    {
        Options = options.Value;
    }

    public DateTime Now => Options.Kind == DateTimeKind.Utc ? DateTime.UtcNow : DateTime.Now;
    public DateTimeKind Kind => Options.Kind;
    public bool SupportsMultipleTimezone => Options.Kind == DateTimeKind.Utc;

    public DateTime Normalize(DateTime dateTime)
    {
        if (Kind == DateTimeKind.Unspecified || Kind == dateTime.Kind)
        {
            return dateTime;
        }

        if (Kind == DateTimeKind.Local && dateTime.Kind == DateTimeKind.Utc)
        {
            return dateTime.ToLocalTime();
        }

        if (Kind == DateTimeKind.Utc && dateTime.Kind == DateTimeKind.Local)
        {
            return dateTime.ToUniversalTime();
        }

        return DateTime.SpecifyKind(dateTime, Kind);
    }
}