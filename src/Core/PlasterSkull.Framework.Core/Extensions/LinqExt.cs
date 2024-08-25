namespace PlasterSkull.Framework.Core;

public static partial class LinqExt
{
    public static TSource? GetCenterItem<TSource>(this IEnumerable<TSource> source)
    {
        int elementsCount = source.Count();
        return elementsCount switch
        {
            > 1 => source.ElementAt((int)Math.Ceiling((double)elementsCount / 2) - 1),
            _ => source.ElementAtOrDefault(0),
        };
    }
}
