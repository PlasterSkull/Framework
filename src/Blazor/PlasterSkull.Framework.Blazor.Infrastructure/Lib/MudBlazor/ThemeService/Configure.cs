using Microsoft.Extensions.DependencyInjection;

namespace PlasterSkull.Framework.Blazor;

partial class Configure
{
    public static IServiceCollection AddPlasterSkullMudThemeService(this IServiceCollection services) =>
        services.AddScoped<IMudThemeService, MudThemeService>();
}
