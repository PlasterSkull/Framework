using PlasterSkull.Framework.Blazor.Demo.Shared.Lib.CodeExampleCacheService;

namespace PlasterSkull.Framework.Blazor.Demo.Shared.Lib;

public static class Configure
{
    public static IServiceCollection ConfigureSharedLayer(this IServiceCollection services) =>
        services
            .AddScoped<PsNavigationManager>()
            .AddCodeExampleCacheService();
}
