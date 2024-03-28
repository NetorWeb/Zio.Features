using System.IO;
using System.Linq;
using Castle.DynamicProxy;

namespace Zio.Features.DI.Autofac;

/// <summary>
/// 方法调用拦截器
/// </summary>
public class CallLoggerInterceptor : IInterceptor
{
    private readonly TextWriter _output;

    public CallLoggerInterceptor(TextWriter output)
    {
        _output = output;
    }

    public void Intercept(IInvocation invocation)
    {
        _output.Write("Calling method {0} with parameters {1}... ",
            invocation.Method.Name,
            string.Join(",", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));

        invocation.Proceed();

        _output.WriteLine("Done：result was {0}.", invocation.ReturnValue);
    }
}