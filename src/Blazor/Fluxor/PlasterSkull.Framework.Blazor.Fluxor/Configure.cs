using Fluxor.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace PlasterSkull.Framework.Blazor.Fluxor;

public static class Configure
{
    public static void UsePlasterSkullActionResolver(this FluxorOptions fluxorOptions) =>
        fluxorOptions.Services.AddTransient<PsFluxorActionResolver>();
}
