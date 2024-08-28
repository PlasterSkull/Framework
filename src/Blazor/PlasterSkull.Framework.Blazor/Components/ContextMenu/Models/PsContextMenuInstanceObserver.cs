using ActualLab.Async;

namespace PlasterSkull.Framework.Blazor;

public sealed class PsContextMenuInstanceObserver
{
    public TagId CallerId => Options.CallerId;

    public required PsContextMenuOptions Options { get; init; }

    public required RenderFragment MenuContent { get; init; }

    #region Initialize instance logic

    private TaskCompletionSource<PsContextMenuInstance> _whenInstanceInitialized =
        TaskCompletionSourceExt.New<PsContextMenuInstance>(); 

    public Task<PsContextMenuInstance> WhenInstanceInitialized =>
        _whenInstanceInitialized.Task;  

    internal void SetInstance(PsContextMenuInstance instance) => 
        _whenInstanceInitialized.TrySetResult(instance);

    #endregion

    #region Events

    public event Func<Task>? OnClosed;

    internal Task NotifyClosed() =>
        OnClosed?.Invoke() ?? 
        Task.CompletedTask;

    #endregion

    #region 

    internal Task RenderAsync() =>
        WhenInstanceInitialized.ContinueWith(task =>
        {
            var instance = task.Result;
            return instance.RenderAsync();
        });

    internal Task FocusAsync() =>
        WhenInstanceInitialized.ContinueWith(task =>
        {
            var instance = task.Result;
            return instance.FocusAsync();
        });

    internal Task PlayHideAnimationAsync() =>
        WhenInstanceInitialized.ContinueWith(task =>
        {
            var instance = task.Result;
            return instance.PlayCloseAnimationAsync();
        });

    internal Task PlayCloseAnimationAsync() =>
        WhenInstanceInitialized.ContinueWith(task =>
        {
            var instance = task.Result;
            return instance.PlayCloseAnimationAsync();
        });

    #endregion
}
