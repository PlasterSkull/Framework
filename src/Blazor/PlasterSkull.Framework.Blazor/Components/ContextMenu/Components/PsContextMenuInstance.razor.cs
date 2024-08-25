namespace PlasterSkull.Framework.Blazor;

public partial class PsContextMenuInstance : PsComponentBase
{
    #region Params

    [CascadingParameter] private PsContextMenuProvider PsContextMenuProvider { get; set; } = null!;

    [Parameter] public Guid Id { get; set; }

    [Parameter] public double X { get; set; }
    [Parameter] public double Y { get; set; }
    [Parameter] public int ZIndex { get; set; }

    [Parameter] public PsContextMenuSettings Settings { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    #endregion

    #region UI Fields

    private ElementReference _selfRef;

    private bool _isMobileSize => PsContextMenuProvider.IsMobileSize;
    private bool _isCurrent => PsContextMenuProvider.IsCurrent(Id);

    private int _overlayZIndex => ZIndex - 1;

    private bool _hidden;
    private bool _closing;

    #endregion

    #region Css/Style

    protected override CssBuilder? ExtendClassNameBuilder =>
        new CssBuilder("d-flex fixed mud-elevation-5")
            .AddClass("active", _isCurrent)
            .AddClass("closing", _closing)
            .AddClass("hidden", _hidden)
            .When(_isMobileSize, mobileSizeBuilder => mobileSizeBuilder
                .AddClass("flex-column w-100 bottom-0 rounded-t-lg")
                .AddClass("h-100", Settings.FullScreenMobile))
            .When(!_isMobileSize, desktopSizeBuilder => desktopSizeBuilder
                .AddClass("left-0 top-0"));

    protected override StyleBuilder? ExtendStyleNameBuilder =>
        new StyleBuilder()
            .AddStyle("background-color", "white")
            .AddStyle("z-index", $"{ZIndex}")
            .When(_isMobileSize, mobileSizeBuilder => mobileSizeBuilder
                .AddStyle("max-height", "calc(100vh - 48px)"))
            .When(!_isMobileSize, desktopSizeBuilder => desktopSizeBuilder
                .AddStyle("transform", GetMenuPosition, true));

    private string GetMenuPosition() =>
        Settings.Origin switch
        {
            Origin.BottomRight => $"translateX(min({X}px, calc(100vw - 100%))) translateY(min({Y}px, calc(100vh - 100%)))",
            Origin.BottomLeft => $"translateX(max(calc({X}px - 100%), 0px)) translateY(min({Y}px, calc(100vh - 100%)))",
            Origin.TopRight => $"translateX(min({X}px, calc(100vw - 100%))) translateY(max(calc({Y}px - 100%), 0px))",
            Origin.TopLeft => $"translateX(max(calc({X}px - 100%), 0px)) translateY(max(calc({Y}px - 100%), 0px))",
            _ => $"translateX(min({X}px, calc(100vw - 100%))) translateY(min({Y}px, calc(100vh - 100%)))"
        };

    private string MenuOverlayClassName =>
        new CssBuilder()
            .AddClass("overlay-hide-animation", _isMobileSize && _closing)
            .Build();

    #endregion

    #region LC Methods

    protected override void OnInitialized()
    {
        DisableAutoRender();

        base.OnInitialized();

        PsContextMenuProvider.RegisterMenuRef(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (!firstRender)
            return;

        await _selfRef.FocusAsync();
        StateHasChanged();
    }

    #endregion

    #region Internal events

    private void OnKeyPress(KeyboardEventArgs args)
    {
        if (args.Key == "Escape")
        {
            PsContextMenuProvider.CloseMenu(Id).CatchAndLog();
            return;
        }
    }

    private void OnBackdropClick() =>
        PsContextMenuProvider.CloseMenu(Id).CatchAndLog();

    #endregion

    #region Public

    public Task Render() => InvokeAsync(StateHasChanged);

    public Task Focus() =>
        InvokeAsync(async () =>
        {
            await _selfRef.FocusAsync();
            StateHasChanged();
        });

    /// <summary>
    /// Обработчик клика итема в контестном меню
    /// </summary>
    /// <param name="ctxMenuCloseBehavior"></param>
    /// <returns>
    /// false - если не будет закрыто
    /// true - если будет закрыто
    /// </returns>
    public bool OnMenuItemClick(PsContextMenuCloseBehavior ctxMenuCloseBehavior)
    {
        if (ctxMenuCloseBehavior is PsContextMenuCloseBehavior.KeepOpened)
            return false;

        if (ctxMenuCloseBehavior is PsContextMenuCloseBehavior.CloseOnlyParent)
        {
            PsContextMenuProvider.CloseMenu(Id).CatchAndLog();
            return true;
        }

        if (ctxMenuCloseBehavior is PsContextMenuCloseBehavior.Close)
        {
            PsContextMenuProvider.CloseAll().CatchAndLog();
            return true;
        }

        return false;
    }

    public async ValueTask SetHiddenState(bool render)
    {
        _hidden = true;

        if (!render)
            return;

        await Render();
    }

    public async ValueTask SetClosedState(bool render)
    {
        _closing = true;

        if (!render)
            return;

        await Render();
    }

    #endregion
}
