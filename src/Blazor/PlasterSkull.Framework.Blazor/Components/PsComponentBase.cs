namespace PlasterSkull.Framework.Blazor;

public abstract class PsComponentBase
    : MudComponentBase
    , IHandleEvent
    , IAsyncDisposable
    , IHasIsDisposed
{
    #region Injects

    [Inject] protected IMudThemeService _mudThemeService { get; init; } = null!;

    #endregion

    #region Fields 

    public TagId TagId { get; private set; }

    private bool _isDisposing;
    public bool IsDisposed { get; private set; }

    #endregion

    #region Css/Style

    private string _defaultRootClassName = string.Empty;
    private string? _customRootClassName;

    protected string RootClassName =>
        _customRootClassName ?? _defaultRootClassName;

    protected virtual CssBuilder? ExtendClassNameBuilder => null;

    protected virtual string ClassName =>
        new CssBuilder(RootClassName)
            .AddClass(() => ExtendClassNameBuilder!.Value, ExtendClassNameBuilder.HasValue)
            .AddClass(() => _renderTracer!.ClassNameBuilder, CanShowRenderInfo)
            .AddClass(Class)
            .Build();

    protected virtual StyleBuilder? ExtendStyleNameBuilder => null;

    protected virtual string StyleName =>
        new StyleBuilder()
            .AddStyle(() => ExtendStyleNameBuilder!.Value, ExtendStyleNameBuilder.HasValue)
            .AddStyle(() => _renderTracer!.StyleNameBuilder, CanShowRenderInfo)
            .AddStyle(Style)
            .Build();

    protected void SetRootClassName(string? rootClassName) =>
        _customRootClassName = rootClassName;

    #endregion

    #region LC Methods

    public override Task SetParametersAsync(ParameterView parameters)
    {
        _defaultRootClassName = GetType().GetRootClassName();

        return base.SetParametersAsync(parameters);
    }

    protected override bool ShouldRender()
    {
        if (CanShowRenderInfo)
        {
            _renderTracer!.OnRenderStart();
            UserAttributes!["render-info"] = _renderTracer!.GetStateMessage();
        }

        return base.ShouldRender();
    }

    protected override Task OnInitializedAsync()
    {
        TagId = TagId.New(RootClassName);

        if (CanShowRenderInfo)
        {
            UserAttributes = new()
            {
                ["render-info"] = _renderTracer!.GetStateMessage(),
            };
        }

        return base.OnInitializedAsync();
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (CanShowRenderInfo)
            _renderTracer!.OnParametersSetCalled();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (!CanShowRenderInfo)
            return;

        _renderTracer!.OnRendered();
    }

    public async ValueTask DisposeAsync()
    {
        if (IsDisposed || _isDisposing)
            return;

        _isDisposing = true;

        await DisposeAsyncCore().ConfigureAwait(false);

        GC.SuppressFinalize(this);
        IsDisposed = true;
    }

    protected virtual ValueTask DisposeAsyncCore()
    {
        if (_renderTracer != null)
        {
            _renderTracer.Dispose();
            _renderTracer = null;
        }

        return ValueTask.CompletedTask;
    }

    #endregion

    #region Render tracing

    private bool _enableComponentRenderTracing;
    private PsRenderTracer? _renderTracer;

    protected bool CanShowRenderInfo =>
        PlasterSkullBlazorGlobal.EnableRenderTracing &&
        _enableComponentRenderTracing &&
        !IsDisposed;

    protected void EnableRenderTracing(PsRenderTracerOptions? _renderTracerOptions = null)
    {
        if (!PlasterSkullBlazorGlobal.EnableRenderTracing || IsDisposed)
            return;

        _enableComponentRenderTracing = true;
        _renderTracer = new(TagId, _renderTracerOptions);
    }

    #endregion

    #region HandleEvent Behaviors

    private bool _disableAutoRender;

    public void DisableAutoRender() =>
        _disableAutoRender = true;

    private async Task CustomCallStateHasChangedOnAsyncCompletion(Task task)
    {
        try
        {
            await task;
        }
        catch // avoiding exception filters for AOT runtime support
        {
            // Ignore exceptions from task cancellations, but don't bother issuing a state change.
            if (task.IsCanceled)
            {
                return;
            }

            throw;
        }

        StateHasChanged();
    }

    Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem callback, object? arg)
    {
        var task = callback.InvokeAsync(arg);
        if (_disableAutoRender)
            return task;

        var shouldAwaitTask =
            task.Status != TaskStatus.RanToCompletion &&
            task.Status != TaskStatus.Canceled;

        StateHasChanged();

        return shouldAwaitTask ?
            CustomCallStateHasChangedOnAsyncCompletion(task) :
            Task.CompletedTask;
    }

    #endregion
}
