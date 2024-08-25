namespace PlasterSkull.Framework.Core;

partial class LinqExt
{
    public static IEnumerable<TSource> TakeIf<TSource>(
        this IEnumerable<TSource> source, 
        bool condition, 
        int count) =>
        condition
            ? source.Take(count)
            : source;

    public static IEnumerable<TSource> TakeIf<TSource>(
        this IEnumerable<TSource> source, 
        bool condition, 
        Func<int> countFunc) =>
        condition
            ? source.Take(countFunc())
            : source;
}
