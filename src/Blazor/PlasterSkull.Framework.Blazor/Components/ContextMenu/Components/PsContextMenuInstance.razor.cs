namespace PlasterSkull.Framework.Blazor;

public partial class PsContextMenuInstance : PsComponentBase
{
    #region Params

    [CascadingParameter] private PsContextMenuProvider PsContextMenuProvider { get; set; } = null!;

    [Parameter] public PsContextMenuOptions Options { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    #endregion

    #region UI Fields

    private ElementReference _selfRef;

    private bool _isMobileSize => PsContextMenuProvider.IsMobileSize;
    private bool _isCurrent => PsContextMenuProvider.IsCurrent(Options.CallerId);

    private TagId _callerId => Options.CallerId;    
    private double _x => Options.X;
    private double _y => Options.Y;
    
    private int _zIndex => Options.ZIndex;  
    private int _overlayZIndex => _zIndex - 1;

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
                .AddClass("h-100", Options.FullScreenMobile))
            .When(!_isMobileSize, desktopSizeBuilder => desktopSizeBuilder
                .AddClass("left-0 top-0"));

    protected override StyleBuilder? ExtendStyleNameBuilder =>
        new StyleBuilder()
            .AddStyle("background-color", "white")
            .AddStyle("z-index", $"{_zIndex}")
            .When(_isMobileSize, mobileSizeBuilder => mobileSizeBuilder
                .AddStyle("max-height", "calc(100vh - 48px)"))
            .When(!_isMobileSize, desktopSizeBuilder => desktopSizeBuilder
                .AddStyle("transform", GetMenuPosition, true));

    private string GetMenuPosition() =>
        Options.Origin switch
        {
            Origin.BottomRight => $"translateX(min({_x}px, calc(100vw - 100%))) translateY(min({_y}px, calc(100vh - 100%)))",
            Origin.BottomLeft => $"translateX(max(calc({_x}px - 100%), 0px)) translateY(min({_y}px, calc(100vh - 100%)))",
            Origin.TopRight => $"translateX(min({_x}px, calc(100vw - 100%))) translateY(max(calc({_y}px - 100%), 0px))",
            Origin.TopLeft => $"translateX(max(calc({_x}px - 100%), 0px)) translateY(max(calc({_y}px - 100%), 0px))",
            _ => $"translateX(min({_x}px, calc(100vw - 100%))) translateY(min({_y}px, calc(100vh - 100%)))"
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
            PsContextMenuProvider.CloseMenuAsync(_callerId).CatchAndLog();
            return;
        }
    }

    private void OnOverlayClosed() =>
        PsContextMenuProvider.CloseMenuAsync(_callerId).CatchAndLog();

    #endregion

    #region Public

    public Task RenderAsync() => InvokeAsync(StateHasChanged);

    public Task FocusAsync() =>
        InvokeAsync(async () =>
        {
            await _selfRef.FocusAsync();
            StateHasChanged();
        });

    internal Task PlayHideAnimationAsync()
    {
        _hidden = true;
        return RenderAsync();
    }

    internal Task PlayCloseAnimationAsync()
    {
        _closing = true;
        return RenderAsync();
    }

    /// <summary>
    /// Обработчик клика итема в контестном меню
    /// </summary>
    /// <param name="contextMenuCloseBehavior"></param>
    /// <returns>
    /// false - если не будет закрыто
    /// true - если будет закрыто
    /// </returns>
    public bool OnMenuItemClick(PsContextMenuCloseBehavior contextMenuCloseBehavior)
    {
        if (contextMenuCloseBehavior is PsContextMenuCloseBehavior.KeepOpened)
            return false;

        if (contextMenuCloseBehavior is PsContextMenuCloseBehavior.CloseOnlyParent)
        {
            PsContextMenuProvider.CloseMenuAsync(_callerId).CatchAndLog();
            return true;
        }

        if (contextMenuCloseBehavior is PsContextMenuCloseBehavior.Close)
        {
            PsContextMenuProvider.CloseAllMenusAsync().CatchAndLog();
            return true;
        }

        return false;
    }

    #endregion
}
