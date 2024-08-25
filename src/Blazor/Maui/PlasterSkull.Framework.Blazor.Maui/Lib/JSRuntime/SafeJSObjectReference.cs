﻿using ActualLab.DependencyInjection;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace PlasterSkull.Framework.Blazor.Maui;

public sealed class SafeJSObjectReference(
    SafeJSRuntime safeJSRuntime,
    IJSObjectReference jsObjectReference)
    : IJSObjectReference,
    IHasIsDisposed
{
    private volatile int _isDisposed;

    internal IJSObjectReference JSObjectReference => jsObjectReference;

    public bool IsDisposed => _isDisposed != 0;

    public ValueTask DisposeAsync() =>
        Interlocked.CompareExchange(ref _isDisposed, 1, 0) != 0 || safeJSRuntime.IsDisconnected
            ? default
            : jsObjectReference.DisposeSilentlyAsync();

    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(SafeJSRuntime.JsonSerialized)] TValue>(
        string identifier, object?[]? args)
    {
        ThrowIfDisposed();
        safeJSRuntime.RequireConnected();
        return jsObjectReference.InvokeAsync<TValue>(identifier, SafeJSRuntime.ToUnsafe(args));
    }

    public ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(SafeJSRuntime.JsonSerialized)] TValue>(
        string identifier, CancellationToken cancellationToken, object?[]? args)
    {
        ThrowIfDisposed();
        safeJSRuntime.RequireConnected();
        return jsObjectReference.InvokeAsync<TValue>(identifier, cancellationToken, SafeJSRuntime.ToUnsafe(args));
    }

    // Protected methods

    private void ThrowIfDisposed() =>
        ObjectDisposedException.ThrowIf(IsDisposed, this);
}
