using Microsoft.JSInterop;
using AWebView = Android.Webkit.WebView;

namespace PlasterSkull.Framework.Blazor.Maui;

partial class MauiBackButtonHandler
{
    public partial async ValueTask HandleAsync(PsBackButtonEventContext context, CancellationToken ct)
    {
        if (_webViewContextProviderWrapper.GetWebView() is not AWebView aWebView)
            return;

        var goBack = await TryGoBack(aWebView).ConfigureAwait(false);
        if (goBack)
            return;

        Platform.CurrentActivity?.MoveTaskToBack(true);
    }

    private async Task<bool> TryGoBack(AWebView webView)
    {
        var canGoBack = webView.CanGoBack();
        if (canGoBack)
        {
            webView.GoBack();
            return true;
        }

        // Sometimes Chromium reports that it can't go back while there are 2 items in the history.
        // It seems that this bug exists for a while, not fixed yet and there is not plans to do it.
        // https://bugs.chromium.org/p/chromium/issues/detail?id=1098388
        // https://github.com/flutter/flutter/issues/59185
        // But we can use web api to navigate back.
        var list = webView.CopyBackForwardList();
        var canGoBack2 = list is { Size: > 1, CurrentIndex: > 0 };
        if (!canGoBack2 || !_webViewContextProviderWrapper.TryGetScopedServices(out var scopedServices))
            return false;

        var js = scopedServices.GetRequiredService<IJSRuntime>();
        await js.InvokeVoidAsync("eval", "history.back()").ConfigureAwait(false);
        return true;
    }
}
