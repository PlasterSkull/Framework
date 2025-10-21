namespace PlasterSkull.Framework.Blazor;

public partial class PsStatefulLoader : ComponentBase
{
    #region Params

    [Parameter] public LoaderState State { get; set; }

    [Parameter] public RenderFragment? NotTriggered { get; set; }
    [Parameter] public RenderFragment? Loading { get; set; }
    [Parameter] public RenderFragment? NoData { get; set; }
    [Parameter] public RenderFragment? Content { get; set; }
    [Parameter] public RenderFragment? Error { get; set; }

    #endregion

    private RenderFragment? GetComponentStateFragment() =>
        State switch
        {
            LoaderState.NotTriggered => NotTriggered,
            LoaderState.Loading => Loading,
            LoaderState.NoData => NoData,
            LoaderState.Content => Content,
            LoaderState.Error => Error,
            _ => Loading
        };
}