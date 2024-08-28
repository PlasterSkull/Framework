using Microsoft.AspNetCore.Components.Routing;
using MudBlazor.Services;

namespace PlasterSkull.Framework.Blazor;

public partial class PsContextMenuProvider
    : PsComponentBase
    , IPsContextMenuProvider
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

    private readonly SemaphoreSlim _actionLocker = new(1, 1);
    private readonly List<PsContextMenuInstanceObserver> _contextMenuObservers = [];
    private IDisposable _locationChangingRegistration = null!;

    private bool _isMobileSize => Breakpoint is Breakpoint.Xs or Breakpoint.Sm;
    private bool _hasOpenedMenu => _contextMenuObservers.Count > 0;
    private const int _startZIndex = 10000;

    #endregion

    #region Css/Style

    protected override CssBuilder? ExtendClassNameBuilder =>
        new CssBuilder()
            .AddClass("d-none", !_hasOpenedMenu)
            .AddClass("d-block w-vw-100 h-vh-100", _hasOpenedMenu);

    protected override StyleBuilder? ExtendStyleNameBuilder =>
        new StyleBuilder()
            .AddStyle("position", "fixed".ImportantCssPropertyValue())
            .AddStyle("z-index", "10000", _hasOpenedMenu);

    #endregion

    #region LC Methods

    protected override void OnInitialized()
    {
        DisableAutoRender();
        EnableRenderTracing();

        base.OnInitialized();

        _psContextMenuService.SetupProvider(this);
        _locationChangingRegistration = _navigationManager.RegisterLocationChangingHandler(OnLocationChanging);
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
        _contextMenuObservers.Clear();
        await _browserViewportService.UnsubscribeAsync(this);
        _psBackButtonService.Unsubscribe(this);
    }

    #endregion

    #region External events

    private ValueTask OnLocationChanging(LocationChangingContext context) => CloseAllMenusAsync();

    #endregion

    #region Internal events

    private Task OnSwipeEnd(SwipeEventArgs args)
    {
        if (args.SwipeDirection is not SwipeDirection.TopToBottom ||
            !_hasOpenedMenu)
            return Task.CompletedTask;

        return CloseMenuAsync(_contextMenuObservers.Last().CallerId).AsTask();
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

        _contextMenuObservers.Clear();

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
        if (_contextMenuObservers.Count == 0)
            return;

        var openedMenu = _contextMenuObservers.Last();

        await InvokeAsync(() => CloseMenuAsync(openedMenu.CallerId).AsTask());

        context.RequestComplete();
    }

    #endregion

    #region Menu methods/fields

    internal Breakpoint Breakpoint { get; private set; }
    public bool IsMobileSize => _isMobileSize;

    internal void RegisterMenuRef(PsContextMenuInstance psContextMenuInstance)
    {
        if (_contextMenuObservers.FirstOrDefault(x => x.CallerId == psContextMenuInstance.Options.CallerId) is not { } observer)
            return;

        observer.SetInstance(psContextMenuInstance);
    }

    internal bool IsCurrent(TagId menuId) =>
        _contextMenuObservers.LastOrDefault()?.CallerId == menuId;

    public async Task<PsContextMenuInstanceObserver> ShowMenuAsync(PsContextMenuOpenArgs args)
    {
        await _actionLocker.WaitAsync();

        try
        {
            if (_contextMenuObservers.FirstOrDefault(x => x.CallerId == args.Options.CallerId) is { } existingObserver)
                return existingObserver;

            var observer = new PsContextMenuInstanceObserver
            {
                Options = args.Options with
                {
                    ZIndex = GetNewMaxZIndex()
                },
                MenuContent = args.MenuContent,
            };
            _contextMenuObservers.Add(observer);

            await InvokeAsync(StateHasChanged);

            await observer.WhenInstanceInitialized;

            return observer;
        }
        finally
        {
            _actionLocker.Release();
        }
    }

    public async ValueTask CloseMenuAsync(TagId menuId)
    {
        await _actionLocker.WaitAsync();

        try
        {
            var observer = _contextMenuObservers.FirstOrDefault(x => x.CallerId == menuId);
            if (observer == null)
                return;

            await observer.PlayCloseAnimationAsync();
            await Task.Delay(190);
            _contextMenuObservers.Remove(observer);

            if (_contextMenuObservers.LastOrDefault() is { } newActiveMenuObserver)
                _ = newActiveMenuObserver.FocusAsync();

            StateHasChanged();
            _ = observer.NotifyClosed();
        }
        finally
        {
            _actionLocker.Release();
        }
    }

    public async ValueTask CloseAllMenusAsync()
    {
        await _actionLocker.WaitAsync();

        try
        {
            if (!_hasOpenedMenu)
                return;

            var activeMenu = _contextMenuObservers.Last();
            await Task.WhenAll(_contextMenuObservers
                .Select(x => x.PlayHideAnimationAsync())
                .Concat([activeMenu.PlayCloseAnimationAsync()]));
            await Task.Delay(190);
            _ = Task.WhenAll(_contextMenuObservers.Select(x => x.NotifyClosed()));
            _contextMenuObservers.Clear();
            StateHasChanged();
        }
        finally
        {
            _actionLocker.Release();
        }
    }

    #endregion

    #region Private

    private int GetNewMaxZIndex() =>
        (_contextMenuObservers.Count == 0
            ? _startZIndex
            : _contextMenuObservers.Max(x => x.Options.ZIndex)) + 2;

    #endregion
}
