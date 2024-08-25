using WebView2Control = Microsoft.UI.Xaml.Controls.WebView2;

namespace PlasterSkull.Framework.Blazor.Maui;

partial class MauiBackButtonHandler
{
    public partial ValueTask HandleAsync(PsBackButtonEventContext context, CancellationToken ct)
    {
        if (_webViewContextProviderWrapper.GetWebView() is not WebView2Control webView2Control ||
            webView2Control.CanGoBack)
            return ValueTask.CompletedTask;

        webView2Control.GoBack();

        // TODO: mb add some logic

        return ValueTask.CompletedTask;
    }
}
