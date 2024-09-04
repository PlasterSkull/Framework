namespace PlasterSkull.Framework.Blazor.Demo.Shared.Lib.CodeExampleCacheService;

public static class Configure
{
    public static IServiceCollection AddCodeExampleCacheService(this IServiceCollection services) =>
        services.AddScoped<ICodeExampleCacheService, Impl.CodeExampleCacheService>();
}
