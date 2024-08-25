using System.Diagnostics.CodeAnalysis;

namespace PlasterSkull.Framework.Blazor.Maui;

internal sealed class WebViewContextProviderWrapper(IWebViewContextProvider? _currentWebViewContextProvider)
{
    public object? GetWebView() =>
        EnsureHasProviderImplementation().GetWebView();
    public bool TryGetScopedServices([NotNullWhen(true)] out IServiceProvider? scopedServices) =>
        EnsureHasProviderImplementation().TryGetScopedServices(out scopedServices);

    private IWebViewContextProvider EnsureHasProviderImplementation() =>
        _currentWebViewContextProvider ??
        throw new NotImplementedException($"{nameof(IWebViewContextProvider)} must be implemented by user");
}
