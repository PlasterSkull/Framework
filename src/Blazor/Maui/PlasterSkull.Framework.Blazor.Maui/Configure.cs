using Microsoft.Maui.LifecycleEvents;

namespace PlasterSkull.Framework.Blazor.Maui;

public static partial class Configure
{
    public static MauiAppBuilder AddEtaiMauiBlazorConfiguration(
        this MauiAppBuilder builder,
        string baseUrl,
        string environment)
    {
        var platform = DeviceInfo.Current.Platform;

        builder.Services.AddSingleton(s => new HostInfo
        {
            AppKind = AppKind.MauiApp,
            ClientKind = true switch
            {
                _ when platform == DevicePlatform.Android => ClientKind.Android,
                _ when platform == DevicePlatform.iOS => ClientKind.iOS,
                _ when platform == DevicePlatform.WinUI => ClientKind.Windows,
                _ when platform == DevicePlatform.macOS => ClientKind.MacCatalyst,
                _ => ClientKind.Unknown,
            },
            BaseUrl = baseUrl,
            Environment = environment,
            DeviceModel = DeviceInfo.Model
        });

        builder.ConfigureLifecycleEvents(ConfigurePlatformLifecycleEvents);

        builder.Services.AddSafeJSRuntime();
        builder.Services.AddSingleton<WebViewContextProviderWrapper>();

        builder.Services.AddPlatformServices();

        return builder;
    }

    private static partial void AddPlatformServices(this IServiceCollection services);
    private static partial void AddPlatformServicesToSkip(HashSet<Type> servicesToSkip);
    private static partial void ConfigurePlatformLifecycleEvents(ILifecycleBuilder events);
}