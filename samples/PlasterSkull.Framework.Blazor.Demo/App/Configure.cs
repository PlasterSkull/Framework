using PlasterSkull.Framework.Blazor.Demo.Shared.Lib;

namespace PlasterSkull.Framework.Blazor.Demo.App;

public static class Configure
{
    public static IServiceCollection ConfigureAppLayer(this IServiceCollection services) =>
        services.ConfigureSharedLayer();
}
