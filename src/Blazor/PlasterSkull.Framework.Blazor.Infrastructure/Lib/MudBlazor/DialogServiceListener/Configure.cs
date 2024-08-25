using Microsoft.Extensions.DependencyInjection;

namespace PlasterSkull.Framework.Blazor;

partial class Configure
{
    public static IServiceCollection AddPlasterSkullMudDialogServiceListener(this IServiceCollection services) =>
        services.AddScoped<IPsDialogServiceListener, PsDialogServiceListener>();
}
