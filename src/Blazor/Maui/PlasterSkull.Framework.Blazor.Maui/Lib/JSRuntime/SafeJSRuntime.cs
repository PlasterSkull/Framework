using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace PlasterSkull.Framework.Blazor.Maui;

// When the page gets reloaded in MAUI app (e.g. due to "Restart" button press there):
// - Blazor.start triggers creation of a new service provider scope for the new page
// - The old scope is disposed, but this might take a while (it's an async disposal)
// - And its IJSRuntime actually continues to work during the disposal -
//   even though JS and element references no longer work there,
//   it can still call JS - but FROM THE NEWLY LOADED PAGE!
//
// The consequences of this are:
// - ActualChat.UI.Blazor.JSObjectReferenceExt.DisposeSilentlyAsync calls may
//   fail with 'JS object instance with ID xxx does not exist'
// - Any invocations of JS methods which don't require JSObjectRef / DotNetObjectRef
//   still continue to work (e.g., think JS methods).
//
// So to address this, we:
// - Manually tag disconnected runtimes via MarkDisconnected
// - Suppress all exceptions that happen due to runtime disconnection
//   in DisposeSilentlyAsync
// - Manually throw JSRuntimeDisconnected from SafeJSRuntime, if it
//   wraps a disconnected JS runtime.
public sealed class SafeJSRuntime(IJSRuntime webViewJSRuntime) : IJSRuntime
{
    internal const DynamicallyAccessedMemberTypes JsonSerialized =
        DynamicallyAccessedMemberTypes.PublicConstructors
        | DynamicallyAccessedMemberTypes.PublicFields
        | DynamicallyAccessedMemberTypes.PublicProperties;

    private volatile int _state; // 1 = ready, 2 = disconnected

    public bool IsDisconnected => _state == 2;
    public bool IsReady => _state != 0;

    internal IJSRuntime WebViewJSRuntime { get; } = webViewJSRuntime;

    public bool MarkReady() =>
        Interlocked.CompareExchange(ref _state, 1, 0) == 0;

    public void MarkDisconnected() =>
        Interlocked.Exchange(ref _state, 2);

    public async ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(JsonSerialized)] TValue>(
        string identifier, object?[]? args)
    {
        try
        {
            var result = await RequireConnected().InvokeAsync<TValue>(identifier, ToUnsafe(args)).ConfigureAwait(false);
            return ToSafe(result);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            if (e is not JSDisconnectedException && IsDisconnected)
                throw JSRuntimeErrors.Disconnected(e);
            throw;
        }
    }

    public async ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(JsonSerialized)] TValue>(
        string identifier, CancellationToken cancellationToken, object?[]? args)
    {
        try
        {
            var result = await RequireConnected().InvokeAsync<TValue>(identifier, cancellationToken, ToUnsafe(args)).ConfigureAwait(false);
            return ToSafe(result);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception e)
        {
            if (e is not JSDisconnectedException && IsDisconnected)
                throw JSRuntimeErrors.Disconnected(e);
            throw;
        }
    }

    public IJSRuntime RequireConnected() =>
        IsDisconnected
            ? throw JSRuntimeErrors.Disconnected()
            : WebViewJSRuntime;

    public static object?[]? ToUnsafe(object?[]? args)
    {
        if (args == null || args.Length == 0)
            return args;

        var containsSafeJSObjectReferences = false;
        var i = 0;
        for (; i < args.Length; i++)
        {
            var arg = args[i];
            if (arg is SafeJSObjectReference)
            {
                containsSafeJSObjectReferences = true;
                break;
            }
        }
        if (!containsSafeJSObjectReferences)
            return args;

        var newArgs = new object?[args.Length];
        for (i = 0; i < args.Length; i++)
            newArgs[i] = ToUnsafe(args[i]);
        return newArgs;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static object? ToUnsafe(object? obj) =>
        obj is SafeJSObjectReference safeJSObjectReference
            ? safeJSObjectReference.JSObjectReference
            : obj;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TValue ToSafe<TValue>(TValue value) =>
        value is not IJSStreamReference && value is IJSObjectReference jsObjectReference
            ? (TValue)(object)new SafeJSObjectReference(this, jsObjectReference)
            : value;
}
