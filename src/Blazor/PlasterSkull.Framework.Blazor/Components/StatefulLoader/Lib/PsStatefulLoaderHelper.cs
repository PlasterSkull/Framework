namespace PlasterSkull.Framework.Blazor;

public static class PsStatefulLoaderHelper
{
    public static bool IsNoDataState(this LoaderState LoaderState) =>
        LoaderState switch
        {
            _ when LoaderState is LoaderState.NotTriggered or LoaderState.NoData => true,
            _ => false,
        };

    public static LoaderState HandleLoaderStates(params LoaderState[] LoaderStates) =>
        LoaderStates switch
        {
            _ when LoaderStates.All(x => x == LoaderState.Content) => LoaderState.Content,
            _ when LoaderStates.All(x => x == LoaderState.NoData) => LoaderState.NoData,
            _ when LoaderStates.All(x => x == LoaderState.NotTriggered) => LoaderState.NotTriggered,
            _ when LoaderStates.Any(x => x == LoaderState.Error) => LoaderState.Error,
            _ => LoaderState.Loading,
        };

    public static LoaderState HandleLoaderStates(Func<bool> func, params LoaderState[] LoaderStates)
    {
        var loaderStateResult = HandleLoaderStates(LoaderStates);

        return loaderStateResult switch
        {
            _ when loaderStateResult == LoaderState.Content && func() => LoaderState.Content,
            _ when loaderStateResult != LoaderState.Content => loaderStateResult,
            _ => LoaderState.Loading
        };
    }
}
