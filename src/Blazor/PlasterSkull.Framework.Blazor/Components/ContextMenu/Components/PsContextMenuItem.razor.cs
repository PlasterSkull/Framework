namespace PlasterSkull.Framework.Blazor;

public partial class PsContextMenuItem : PsComponentBase
{
    #region Params

    [CascadingParameter] private PsContextMenuInstance PsContextMenuInstance { get; set; } = null!;

    [Parameter] public string? Text { get; set; }
    [Parameter] public Typo TextTypo { get; set; } = Typo.body1;
    [Parameter] public string? Icon { get; set; }
    [Parameter] public Color Color { get; set; } = Color.Primary;
    [Parameter] public string? HexColor { get; set; }
    [Parameter] public bool Selected { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool Hover { get; set; } = true;
    [Parameter] public bool FullWidth { get; set; } = true;
    [Parameter] public PsContextMenuCloseBehavior CloseBehavior { get; set; }

    [Parameter] public string? TextClass { get; set; }
    [Parameter] public string? TextStyle { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public EventCallback OnClick { get; set; }

    #endregion    

    #region LC Methods

    protected override void OnInitialized()
    {
        DisableAutoRender();

        base.OnInitialized();
    }

    #endregion

    #region Internal events

    private void InternalOnClick()
    {
        if (Disabled)
            return;

        OnClick.InvokeAsync().CatchAndLog();

        if (PsContextMenuInstance != null && PsContextMenuInstance.OnMenuItemClick(CloseBehavior))
            return;

        StateHasChanged();
    }

    #endregion
}
