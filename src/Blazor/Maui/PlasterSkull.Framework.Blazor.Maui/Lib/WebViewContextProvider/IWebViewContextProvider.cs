using System.Diagnostics.CodeAnalysis;

namespace PlasterSkull.Framework.Blazor.Maui;

public interface IWebViewContextProvider
{
    object? GetWebView();
    bool TryGetScopedServices([NotNullWhen(true)] out IServiceProvider? scopedServices);
}
