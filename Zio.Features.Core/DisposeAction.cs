using System;

namespace Zio.Features.Core;

public class DisposeAction : IDisposable
{
    private readonly Action _action;

    public DisposeAction(Action action)
    {
        Check.NotNull(action, nameof(action));

        _action = action;
    }

    public void Dispose()
    {
        _action();
    }
}

public class DisposeAction<T> : IDisposable
{

    private readonly Action<T> _action;

    private readonly T? _parameter;

    public DisposeAction(Action<T> action, T? parameter)
    {

        Check.NotNull(action, nameof(action));

        _action = action;
        _parameter = parameter;
    }

    public void Dispose()
    {
        if (_parameter != null)
        {
            _action(_parameter);
        }
    }
}