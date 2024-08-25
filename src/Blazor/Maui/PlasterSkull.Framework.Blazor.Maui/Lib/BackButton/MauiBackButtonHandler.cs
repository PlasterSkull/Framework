
namespace PlasterSkull.Framework.Blazor.Maui;

internal sealed partial class MauiBackButtonHandler(
    WebViewContextProviderWrapper _webViewContextProviderWrapper)
    : IPsBackButtonClickNativeHandler
{
    public partial ValueTask HandleAsync(PsBackButtonEventContext context, CancellationToken ct = default);
}
