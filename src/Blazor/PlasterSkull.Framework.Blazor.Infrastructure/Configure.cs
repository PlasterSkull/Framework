using Microsoft.Extensions.DependencyInjection;

namespace PlasterSkull.Framework.Blazor;

public static partial class Configure
{
    public static IServiceCollection AddPlasterSkullInfrastructureServices(this IServiceCollection services) =>
        services
            .AddScoped<PsScopedServicesStarter>()
            .AddPlasterSkullBackButtonService()
            .AddPlasterSkullMudDialogServiceListener();
}
