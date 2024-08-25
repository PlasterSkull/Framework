using Microsoft.Extensions.DependencyInjection;

namespace PlasterSkull.Framework.Blazor;

partial class Configure
{
    public static IServiceCollection AddPlasterSkullContextMenuServices(this IServiceCollection services) =>
        services.AddScoped<IPsContextMenuService, PsContextMenuService>();
}
