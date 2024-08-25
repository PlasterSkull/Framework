namespace PlasterSkull.Framework.Blazor;

public static class EventHelper
{
    public static Action WithoutRender(Action callback) =>
        new SyncHandler(callback).Invoke;

    public static Action<TValue> WithoutRender<TValue>(Action<TValue> callback) =>
        new SyncHandler<TValue>(callback).Invoke;

    public static Func<Task> WithoutRender(Func<Task> callback) =>
        new AsyncHandler(callback).Invoke;

    public static Func<TValue, Task> WithoutRender<TValue>(Func<TValue, Task> callback) =>
        new AsyncHandler<TValue>(callback).Invoke;

    private record SyncHandler(Action callback) : HandlerBase
    {
        public void Invoke() => callback();
    }

    private record SyncHandler<T>(Action<T> callback) : HandlerBase
    {
        public void Invoke(T arg) => callback(arg);
    }

    private record AsyncHandler(Func<Task> callback) : HandlerBase
    {
        public Task Invoke() => callback();
    }

    private record AsyncHandler<T>(Func<T, Task> callback) : HandlerBase
    {
        public Task Invoke(T arg) => callback(arg);
    }

    private record HandlerBase : IHandleEvent
    {
        public Task HandleEventAsync(EventCallbackWorkItem item, object? arg) =>
            item.InvokeAsync(arg);
    }
}
