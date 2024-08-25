using Microsoft.Extensions.DependencyInjection;

namespace PlasterSkull.Framework.Blazor;

partial class Configure
{
    public static IServiceCollection AddPlasterSkullBackButtonService(this IServiceCollection services) =>
        services.AddScoped<IPsBackButtonService, PsBackButtonService>();
}
