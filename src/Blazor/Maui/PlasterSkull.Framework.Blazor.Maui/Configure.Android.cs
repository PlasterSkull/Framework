using Microsoft.Maui.LifecycleEvents;

namespace PlasterSkull.Framework.Blazor.Maui;

partial class Configure
{
    private static partial void AddPlatformServices(this IServiceCollection services) { }

    private static partial void AddPlatformServicesToSkip(HashSet<Type> servicesToSkip) { }

    private static partial void ConfigurePlatformLifecycleEvents(ILifecycleBuilder events) { }
}
