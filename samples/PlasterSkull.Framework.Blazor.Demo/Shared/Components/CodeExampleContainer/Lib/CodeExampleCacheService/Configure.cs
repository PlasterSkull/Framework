namespace PlasterSkull.Framework.Blazor.Demo.Shared;

partial class Configure
{
    public static IServiceCollection AddCodeExampleCacheService(this IServiceCollection services) =>
        services.AddScoped<ICodeExampleCacheService, CodeExampleCacheService>();
}
