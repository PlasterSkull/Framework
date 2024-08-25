
namespace PlasterSkull.Framework.Blazor.Maui;

partial class MauiBackButtonHandler
{
    public partial ValueTask HandleAsync(PsBackButtonEventContext context, CancellationToken ct)
    {
        // TODO

        _ = _webViewContextProviderWrapper.GetWebView();

        return ValueTask.CompletedTask;
    }
}
