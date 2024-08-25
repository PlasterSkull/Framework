namespace PlasterSkull.Framework.Core;

partial class LinqExt
{
    public static IEnumerable<TSource> WhereIf<TSource>(
        this IEnumerable<TSource> source,
        bool condition,
        Func<TSource, bool> predicate)
        where TSource : class =>
        condition ? source.Where(predicate) : source;

    public static IEnumerable<TSource> When<TSource>(
        this IEnumerable<TSource> source,
        bool condition,
        Func<IEnumerable<TSource>, IEnumerable<TSource>> whenFunc)
        where TSource : class =>
        condition ? whenFunc(source) : source;
}
