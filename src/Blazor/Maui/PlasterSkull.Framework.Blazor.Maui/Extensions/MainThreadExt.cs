namespace PlasterSkull.Framework.Blazor.Maui;

public static class MainThreadExt
{
    // Thread pool scheduler -> Main thread
    public static void InvokeLater(Action action) =>
        _ = Task.Run(() => MainThread.BeginInvokeOnMainThread(action));

    // Thread pool scheduler -> Main thread
    public static Task InvokeLaterAsync(Action action) =>
        Task.Run(() => MainThread.InvokeOnMainThreadAsync(action));

    // Thread pool scheduler -> Main thread
    public static Task InvokeLaterAsync(Func<Task> action) =>
        Task.Run(() => MainThread.InvokeOnMainThreadAsync(action));

    // Thread pool scheduler -> Main thread
    public static Task<T> InvokeLaterAsync<T>(Func<Task<T>> action) =>
        Task.Run(() => MainThread.InvokeOnMainThreadAsync(action));
}
