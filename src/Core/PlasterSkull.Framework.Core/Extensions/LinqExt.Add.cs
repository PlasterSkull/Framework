using System.Collections.Immutable;

namespace PlasterSkull.Framework.Core;

partial class LinqExt
{
    public static IEnumerable<TSource> AppendIf<TSource>(
        this IEnumerable<TSource> source, 
        bool condition, 
        TSource element) =>
        condition
            ? source.Append(element)
            : source;

    public static ImmutableList<TSource> AddIf<TSource>(
        this ImmutableList<TSource> source,
        bool condition,
        TSource element) =>
        condition
            ? source.Add(element)
            : source;

    public static ImmutableList<TSource> AddIf<TSource>(
        this ImmutableList<TSource> source,
        bool condition,
        Func<TSource> elementFunc) =>
        condition
            ? source.Add(elementFunc())
            : source;

    public static ImmutableList<TSource> AddRangeIf<TSource>(
        this ImmutableList<TSource> source,
        bool condition,
        IEnumerable<TSource> elements) =>
        condition
            ? source.AddRange(elements)
            : source;

    public static ImmutableList<TSource> AddRangeIf<TSource>(
        this ImmutableList<TSource> source,
        bool condition,
        Func<IEnumerable<TSource>> elementsFunc) =>
        condition
            ? source.AddRange(elementsFunc())
            : source;
}
