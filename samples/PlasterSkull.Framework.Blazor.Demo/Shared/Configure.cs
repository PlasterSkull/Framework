namespace PlasterSkull.Framework.Blazor.Demo.Shared;

public static partial class Configure
{
    public static IServiceCollection ConfigureSharedLayer(this IServiceCollection services) =>
        services
            .AddScoped<PsNavigationManager>()
            .AddCodeExampleCacheService();
}
