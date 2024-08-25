namespace PlasterSkull.Framework.Core;

partial class LinqExt
{
    public static IEnumerable<T> SelectRecursive<T>(
        this T source,
        Func<T, T?> navigationFunc)
        where T : class
    {
        if (source == null)
            yield break;

        yield return source;

        var recursiveSequence = navigationFunc(source)?.SelectRecursive(navigationFunc);

        if (recursiveSequence == null)
            yield break;

        foreach (var recursiveItem in recursiveSequence)
            yield return recursiveItem;
    }

    public static IEnumerable<T> SelectManyRecursive<T>(
        this T source,
        Func<T, IEnumerable<T>> recursiveFunc)
        where T : class
    {
        if (source == null)
            yield break;

        var recursiveSequence = recursiveFunc(source)?.SelectManyRecursive(recursiveFunc);

        if (recursiveSequence == null)
            yield break;
        
        foreach (var recursiveItem in recursiveSequence)
            yield return recursiveItem;
    }

    public static IEnumerable<T> SelectManyRecursive<T>(
        this IEnumerable<T> source,
        Func<T, IEnumerable<T>> recursiveFunc)
        where T : class
    {
        if (source == null)
            yield break;

        foreach (var mainItem in source)
        {
            yield return mainItem;

            var recursiveSequence = recursiveFunc(mainItem)?.SelectManyRecursive(recursiveFunc);

            if (recursiveSequence == null)           
                continue;
            
            foreach (var recursiveItem in recursiveSequence)
                yield return recursiveItem;
        }
    }
}
