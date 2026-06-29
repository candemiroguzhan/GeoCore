namespace GeoCore.Shared.Extensions;

public static class EnumerableExtensions
{
    public static IReadOnlyList<T> EmptyIfNull<T>(this IEnumerable<T>? source)
    {
        return source?.ToArray() ?? [];
    }
}
