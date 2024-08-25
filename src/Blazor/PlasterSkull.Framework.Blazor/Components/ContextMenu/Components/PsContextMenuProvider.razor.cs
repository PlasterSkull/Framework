using Microsoft.AspNetCore.Components.Routing;
using MudBlazor.Services;

namespace PlasterSkull.Framework.Blazor;

public partial class PsContextMenuProvider
    : PsComponentBase
    , IBrowserViewportObserver
    , IPsBackButtonObserver
{
    #region Injects

    [Inject] NavigationManager _navigationManager { get; init; } = null!;
    [Inject] IPsContextMenuService _psContextMenuService { get; init; } = null!;
    [Inject] IPsBackButtonService _psBackButtonService { get; init; } = null!;
    [Inject] IBrowserViewportService _browserViewportService { get; init; } = null!;

    #endregion

    #region Fields

    private readonly Dictionary<Guid, PsContextMenuInstanceObserver> _contextMenus = [];
    private IDisposable _locationChangingRegistration = null!;

    private bool _isMobileSize => Breakpoint is Breakpoint.Xs or Breakpoint.Sm;
    private bool _closingState;

    #endregion

    #region Css/Style

    protected override CssBuilder? ExtendClassNameBuilder =>
        new CssBuilder()
            .AddClass("d-none", _contextMenus.Count == 0)
            .AddClass("d-block w-vw-100 h-vh-100", _contextMenus.Count > 0);

    protected override StyleBuilder? ExtendStyleNameBuilder =>
        new StyleBuilder()
            .AddStyle("position", "fixed".ImportantCssPropertyValue())
            .AddStyle("z-index", "10000", _contextMenus.Count > 0);

    #endregion

    #region LC Methods

    protected override void OnInitialized()
    {
        DisableAutoRender();
        EnableRenderTracing();

        base.OnInitialized();

        _locationChangingRegistration = _navigationManager.RegisterLocationChangingHandler(OnLocationChanging);

        _psContextMenuService.OnShowRequest += OnShowRequest;
        _psContextMenuService.OnRenderRequest += OnRenderRequest;
        _psContextMenuService.OnHideRequest += OnHideRequest;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (!firstRender)
            return;

        Breakpoint = await _browserViewportService.GetCurrentBreakpointAsync();
        await _browserViewportService.SubscribeAsync(this);
        _psBackButtonService.Subscribe(this);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await base.DisposeAsyncCore();

        _locationChangingRegistration.Dispose();

        _psContextMenuService.OnShowRequest -= OnShowRequest;
        _psContextMenuService.OnRenderRequest -= OnRenderRequest;
        _psContextMenuService.OnHideRequest -= OnHideRequest;

        await _browserViewportService.UnsubscribeAsync(this);
        _psBackButtonService.Unsubscribe(this);
    }

    #endregion

    #region External events

    private ValueTask OnLocationChanging(LocationChangingContext context) => CloseAll();

    private void OnShowRequest(PsContextMenuOpenArgs args)
    {
        if (args.RenderFragment == null) return;

        _contextMenus.Add(args.MenuId, new PsContextMenuInstanceObserver
        {
            Id = args.MenuId,

            X = args.X,
            Y = args.Y,
            Settings = args.Settings,

            MenuContent = args.RenderFragment,

            OnHiding = args.OnHiding,
        });

        this.NotifyStateHasChanged();
    }

    private void OnRenderRequest(Guid menuId) =>
        _contextMenus.GetValueOrDefault(menuId)?.Instance?.Render();

    private void OnHideRequest(Guid menuId) =>
        CloseMenu(menuId).AsTask();

    #endregion

    #region Internal events

    private Task OnSwipeEnd(SwipeEventArgs args)
    {
        if (args.SwipeDirection is not SwipeDirection.TopToBottom ||
            _contextMenus.Count == 0)
            return Task.CompletedTask;

        return CloseMenu(_contextMenus.Last().Key).AsTask();
    }

    #endregion

    #region BrowserViewportObserver

    Guid IBrowserViewportObserver.Id => TagId.Value;

    ResizeOptions IBrowserViewportObserver.ResizeOptions { get; } = new()
    {
        NotifyOnBreakpointOnly = true,
    };

    Task IBrowserViewportObserver.NotifyBrowserViewportChangeAsync(BrowserViewportEventArgs browserViewportEventArgs)
    {
        Breakpoint = browserViewportEventArgs.Breakpoint;

        _contextMenus.Clear();

        this.NotifyStateHasChanged();
        return Task.CompletedTask;
    }

    #endregion

    #region BackButtonObserver

    private static readonly PsBackButtonObserverInfo s_backButtonObserverInfo = new()
    {
        Priority = PsBackButtonDefaultPriorities.Default,
    };

    TagId IPsBackButtonObserver.TagId => TagId;

    PsBackButtonObserverInfo IPsBackButtonObserver.BackButtonObserverInfo => s_backButtonObserverInfo;

    async ValueTask IPsBackButtonObserver.NotifyBackButtonEventAsync(PsBackButtonEventContext context, CancellationToken ct)
    {
        if (_contextMenus.Count == 0)
            return;

        var openedMenu = _contextMenus.Last();

        await InvokeAsync(() => CloseMenu(openedMenu.Key).AsTask());

        context.RequestComplete();
    }

    #endregion

    #region Menu methods/fields

    internal Breakpoint Breakpoint { get; private set; }
    public bool IsMobileSize => _isMobileSize;

    internal void RegisterMenuRef(PsContextMenuInstance pSContextMenuInstance)
    {
        if (!_contextMenus.TryGetValue(pSContextMenuInstance.Id, out var observer))
            return;

        observer.SetInstance(pSContextMenuInstance);
    }

    internal bool IsCurrent(Guid menuId) =>
        _contextMenus.LastOrDefault().Key == menuId;

    // NOTE (Ivan Stuk)
    // В CloseMenu и CloseAll есть установка состояний менюшек и рендер после этого. 
    // Нужно это для анимации скрытия менюшки. Трабл в том, что я не нашёл как сделать анимацию
    // при уничтожении блока через css, поэтому пришлось прибегнуть к подобному. Delay на 190мс нужен для того,
    // чтобы отработала анимация сокрытия в 200мс. 10 мс специально оставил, чтобы следующие инструкции выполнились
    // примерно под конец анимации
    internal async ValueTask CloseMenu(Guid menuId)
    {
        if (!_contextMenus.TryGetValue(menuId, out var menu) || _closingState)
            return;

        _closingState = true;

        menu.OnHiding?.Invoke().CatchAndLog();
        menu.Instance?.SetClosedState(render: true).CatchAndLog();
        await Task.Delay(190);

        _contextMenus.Remove(menuId);
        if (_contextMenus.Count != 0)
            _contextMenus.GetValueOrDefault(_contextMenus.Last().Key)?.Instance?.Focus();

        _closingState = false;
        StateHasChanged();
    }

    internal async ValueTask CloseAll()
    {
        if (_contextMenus.Count == 0 || _closingState)
            return;

        _closingState = true;

        var activeMenu = _contextMenus.Last().Value;
        await Task.WhenAll(_contextMenus.Values
            .Except([activeMenu])
            .Select(x => x.Instance?.SetHiddenState(render: false) ?? ValueTask.CompletedTask)
            .Concat([activeMenu.Instance?.SetClosedState(render: false) ?? ValueTask.CompletedTask])
            .Select(x => x.AsTask()));
        StateHasChanged();
        await Task.Delay(190);

        _closingState = false;
        _contextMenus.Clear();
        StateHasChanged();
    }

    #endregion
}
