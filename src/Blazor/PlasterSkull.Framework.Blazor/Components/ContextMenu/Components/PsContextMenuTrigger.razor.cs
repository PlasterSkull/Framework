
namespace PlasterSkull.Framework.Blazor;

public partial class PsContextMenuTrigger : PsContextMenuTriggerComponentBase
{
    #region Params

    [Parameter] public bool ShowTriggerBody { get; set; } = true;

    #endregion

    #region Injects

    [Inject] IPsContextMenuService _psContextMenuService { get; init; } = null!;

    #endregion

    #region UI Fields

    private PsContextMenuInstanceObserver? _observer;
    private bool _isOpened =>
        _observer != null;

    private bool _clickPropagation =>
        ActivationEvent is PsContextMenuActivateBehavior.MouseBoth or PsContextMenuActivateBehavior.MouseLeftClick &&
        ClickPropagation;

    private bool _contextMenuPropagation =>
        ActivationEvent is PsContextMenuActivateBehavior.MouseBoth or PsContextMenuActivateBehavior.MouseRightClick &&
        ClickPropagation;

    private bool _preventContextMenuBehavior =>
        ActivationEvent is PsContextMenuActivateBehavior.MouseBoth or PsContextMenuActivateBehavior.MouseRightClick;

    #endregion

    #region Css/Style

    protected override CssBuilder? ExtendClassNameBuilder =>
        new CssBuilder()
            .AddClass(ShownClass, _isOpened)
            .AddClass(HiddenClass, !_isOpened);

    #endregion

    #region LC Methods

    public override Task SetParametersAsync(ParameterView parameters)
    {
        if (_isOpened)
            _ = _observer!.RenderAsync();

        return base.SetParametersAsync(parameters);
    }

    protected override void OnInitialized()
    {
        DisableAutoRender();

        base.OnInitialized();
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await base.DisposeAsyncCore();

        if (_isOpened)
        {
            _observer!.OnClosed -= OnObserverClosed;
            _observer = null;
            _ = _psContextMenuService.CloseMenuAsync(TagId);
        }
    }

    #endregion

    #region External events

    private Task OnObserverClosed()
    {
        _observer!.OnClosed -= OnObserverClosed;
        _observer = null;
        return OnClosed.InvokeAsync();
    }

    #endregion

    #region Internal events

    private Task OnClick(MouseEventArgs args)
    {
        if (Disabled)
            return Task.CompletedTask;

        bool isContextMenuLongTap = !IgnoreLongTap && args.Button == -1 && args.Type == "contextmenu";
        bool canRequestShow =
            ActivationEvent is PsContextMenuActivateBehavior.MouseBoth ||
            ActivationEvent is PsContextMenuActivateBehavior.MouseRightClick && (args.Button == 2 || isContextMenuLongTap) ||
            ActivationEvent is PsContextMenuActivateBehavior.MouseLeftClick && args.Button == 0;
        if (!canRequestShow)
            return Task.CompletedTask;

        return ShowMenuAsync(args);
    }

    #endregion

    #region Public

    public async Task ShowMenuAsync(MouseEventArgs args)
    {
        if (Disabled)
            return;

        _observer = await _psContextMenuService.ShowMenuAsync(new PsContextMenuOpenArgs
        {
            Options = new()
            {
                CallerId = TagId,
                X = args.ClientX,
                Y = args.ClientY,
                Title = Title,
                FullScreenMobile = FullScreenMobile,
                ShowMobileHeader = HideMobileHeader,
                Origin = Origin,
            },
            MenuContent = MenuContent ?? null!,
        });
        _observer.OnClosed += OnObserverClosed;

        await OnOpened.InvokeAsync();
    }

    public ValueTask CloseMenuAsync() =>
        _psContextMenuService.CloseMenuAsync(TagId);

    public Task RenderMenuAsync() =>
        _isOpened
            ? _observer!.RenderAsync()
            : Task.CompletedTask;

    #endregion
}
