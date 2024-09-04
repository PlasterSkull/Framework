namespace PlasterSkull.Framework.Blazor;

public partial class PsListItem : PsComponentBase
{
    #region Params

    [Parameter] public string? Text { get; set; }
    [Parameter] public Typo TextTypo { get; set; } = Typo.body1;
    [Parameter] public string? Icon { get; set; }
    [Parameter] public Color Color { get; set; } = Color.Inherit;
    [Parameter] public string? HexColor { get; set; }
    [Parameter] public bool Selected { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool Hover { get; set; }
    [Parameter] public bool FullWidth { get; set; } = true;

    [Parameter] public string? TextClass { get; set; }
    [Parameter] public string? TextStyle { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? OptionContent { get; set; }

    [Parameter] public EventCallback OnClick { get; set; }

    #endregion

    #region Css/Style

    protected override CssBuilder? ExtendClassNameBuilder =>
        new CssBuilder()
            .AddClass("w-100", FullWidth)
            .AddClass("pointer-events-none", Disabled)
            .AddClass("mud-primary-lighten-hover", Hover)
            .AddClass("mud-primary-lighten", Selected && HexColor.IsNullOrEmpty());

    protected override StyleBuilder? ExtendStyleNameBuilder =>
        new StyleBuilder()
            .AddStyle(
                "background-color",
                () => new MudColor(HexColor!).SetAlpha(0.2).ToHexA(),
                Selected && !HexColor.IsNullOrEmpty());

    private string TextClassName =>
        new CssBuilder()
            .AddClass("text-ellipsis")
            .AddClass(Color.GetColorCssClass(), HexColor.IsNullOrEmpty())
            .AddClass(TextClass)
            .Build();

    private string TextStyleName =>
        new StyleBuilder()
            .AddStyle("color", HexColor, !string.IsNullOrEmpty(HexColor))
            .AddStyle(TextStyle)
            .Build();

    #endregion

    #region LC Methods

    protected override void OnInitialized()
    {
        DisableAutoRender();

        base.OnInitialized();
    }

    #endregion

    #region Internal events

    private Task InternalOnClick()
    {
        if (Disabled)
            return Task.CompletedTask;

        return OnClick.InvokeAsync();
    }

    #endregion
}
