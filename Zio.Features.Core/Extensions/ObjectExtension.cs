namespace Zio.Features.Core.Extensions;

public static class ObjectExtension
{
    public static T As<T>(this object obj)
        where T : class
    {
        return (T)obj;
    }
}