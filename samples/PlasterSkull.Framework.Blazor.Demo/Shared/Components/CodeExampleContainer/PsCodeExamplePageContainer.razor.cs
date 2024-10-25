namespace PlasterSkull.Framework.Blazor.Demo.Shared;

public partial class PsCodeExamplePageContainer : PsComponentBase
{
    #region Params

    [Parameter, EditorRequired] public required string Title { get; set; }

    [Parameter, EditorRequired] public required RenderFragment ChildContent { get; set; }

    #endregion

    #region Fields

    private string _pageTitle = null!;

    #endregion

    #region LC Events

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _pageTitle = $"{Title} | PlasterSkull";
    }

    #endregion
}
