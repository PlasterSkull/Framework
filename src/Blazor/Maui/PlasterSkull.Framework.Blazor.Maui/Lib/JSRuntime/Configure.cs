using Microsoft.JSInterop;

namespace PlasterSkull.Framework.Blazor.Maui;

partial class Configure
{
    public static IServiceCollection AddSafeJSRuntime(this IServiceCollection services)
    {
        var jsRuntimeRegistration = services.FirstOrDefault(c => c.ServiceType == typeof(IJSRuntime));
        if (jsRuntimeRegistration == null)
        {
            //DefaultLog.LogWarning("IJSRuntime registration is not found. Can't override WebViewJSRuntime");
            return services;
        }
        var webViewJSRuntimeType = jsRuntimeRegistration.ImplementationType;
        if (webViewJSRuntimeType == null)
        {
            //DefaultLog.LogWarning("IJSRuntime registration has no ImplementationType. Can't override WebViewJSRuntime");
            return services;
        }
        services.Remove(jsRuntimeRegistration);
        services.Add(new ServiceDescriptor(
            typeof(SafeJSRuntime),
            svp => new SafeJSRuntime((IJSRuntime)ActivatorUtilities.CreateInstance(svp, webViewJSRuntimeType)),
            jsRuntimeRegistration.Lifetime));
        services.Add(new ServiceDescriptor(
            typeof(IJSRuntime),
            svp =>
            {
                var safeJSRuntime = svp.GetRequiredService<SafeJSRuntime>();
                if (!safeJSRuntime.IsReady)
                {
                    // In MAUI Hybrid Blazor IJSRuntime service is resolved first time from PageContext and casted to WebViewJSRuntime,
                    // to being attached with WebView. So we need to return original WebViewJSRuntime instance.
                    // After that we can return 'safe' IJSRuntime implementation.
                    // See https://github.com/dotnet/aspnetcore/blob/410efd482f494d1ab05ce25b932b5788699c2308/src/Components/WebView/WebView/src/PageContext.cs#L44
                    safeJSRuntime.MarkReady();
                    return safeJSRuntime.WebViewJSRuntime;
                }
                // After that there is no more bindings with implementation type, so we can return protected JSRuntime.
                return safeJSRuntime;
            },
            ServiceLifetime.Transient));

        return services;
    }
}
