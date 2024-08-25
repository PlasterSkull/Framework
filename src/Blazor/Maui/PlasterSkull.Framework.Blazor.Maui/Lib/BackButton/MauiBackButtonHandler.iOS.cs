using WebKit;

namespace PlasterSkull.Framework.Blazor.Maui;

partial class MauiBackButtonHandler
{
    public partial ValueTask HandleAsync(PsBackButtonEventContext context, CancellationToken ct)
    {
        if (_webViewContextProviderWrapper.GetWebView() is not WKWebView wKWebView ||
            wKWebView.CanGoBack)
            return ValueTask.CompletedTask;

        wKWebView.GoBack();

        // TODO: mb add some logic

        return ValueTask.CompletedTask;
    }
}
