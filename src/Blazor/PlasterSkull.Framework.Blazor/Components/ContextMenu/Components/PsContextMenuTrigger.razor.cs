
namespace PlasterSkull.Framework.Blazor;

public partial class PsContextMenuTrigger : PsContextMenuTriggerComponentBase
{
    #region Params

    [Parameter] public bool ShowTriggerBody { get; set; }

    #endregion

    #region Injects

    [Inject] IPsContextMenuService _psContextMenuService { get; init; } = null!;

    #endregion

    #region UI Fields

    private readonly Guid _menuId = Guid.NewGuid();

    private bool _clickPropagation =>
        ActivationEvent is PsContextMenuActivateBehavior.MouseBoth or PsContextMenuActivateBehavior.MouseLeftClick &&
        ClickPropagation;

    private bool _contextMenuPropagation =>
        ActivationEvent is PsContextMenuActivateBehavior.MouseBoth or PsContextMenuActivateBehavior.MouseRightClick &&
        ClickPropagation;

    private bool _preventContextMenuBehavior =>
        ActivationEvent is PsContextMenuActivateBehavior.MouseBoth or PsContextMenuActivateBehavior.MouseRightClick;

    private bool _isOpen;

    #endregion

    #region Css/Style

    protected override CssBuilder? ExtendClassNameBuilder =>
        new CssBuilder()
            .AddClass(ShownClass, _isOpen)
            .AddClass(HiddenClass, !_isOpen);

    #endregion

    #region LC Methods

    public override Task SetParametersAsync(ParameterView parameters)
    {
        if (_isOpen)
            _psContextMenuService.Render(_menuId);

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

        if (_isOpen)
            _psContextMenuService.Hide(_menuId);
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

        return ShowMenu(args);
    }

    #endregion

    #region Public

    public async Task ShowMenu(MouseEventArgs args)
    {
        if (Disabled)
            return;

        await OnAppearing.InvokeAsync();

        _psContextMenuService.Show(new PsContextMenuOpenArgs
        {
            MenuId = _menuId,
            RenderFragment = MenuContent,
            X = args.ClientX,
            Y = args.ClientY,
            Settings = new()
            {
                Title = Title,
                FullScreenMobile = FullScreenMobile,
                ShowMobileHeader = HideMobileHeader,
                Origin = Origin,
            },
            OnHiding = OnMenuHide
        });

        _isOpen = true;
    }

    public void HideMenu() =>
        _psContextMenuService.Hide(_menuId);

    public void RenderMenu() =>
        _psContextMenuService.Render(_menuId);

    #endregion

    #region Private Methods

    private Task OnMenuHide()
    {
        _isOpen = false;
        return OnHiding.InvokeAsync();
    }

    #endregion
}
