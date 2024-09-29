namespace Vn.Utils;

public static class NullableExtensions
{
    public static void IfSome<T>(this T? value, Action<T> action) where T : class
    {
        if (value is not null)
        {
            action(value);
        }
    }

    public static void IfNull<T>(this T? value, Action action) where T : class
    {
        if (value is null)
        {
            action();
        }
    }

    public static void IfSome<T>(this T? value, Action<T> action) where T : struct
    {
        if (value.HasValue)
        {
            action(value.Value);
        }
    }

    public static void IfNull<T>(this T? value, Action action) where T : struct
    {
        if (!value.HasValue)
        {
            action();
        }
    }

    public static void Match<T>(this T? value, Action<T> ifSome, Action ifNone)
    {
        if (value is not null)
        {
            ifSome(value);
        }
        else
        {
            ifNone();
        }
    }

    public static U Match<T, U>(this T? value, Func<T, U> ifSome, Func<U> ifNone) =>
        value is not null ? ifSome(value) : ifNone();

    public static bool IsSome<T>(this T? value) => value is not null;
}