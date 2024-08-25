using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace PlasterSkull.Framework.Blazor;

public static partial class Configure
{
    public static IServiceCollection AddPlasterSkullServices(
        this IServiceCollection services) =>
        services
            .AddMudServices()
            .AddPlasterSkullServicesCore();

    public static IServiceCollection AddPlasterSkullServices(
        this IServiceCollection services,
        Action<MudServicesConfiguration> mudServicesConfiguration) =>
        services
            .AddMudServices(mudServicesConfiguration)
            .AddPlasterSkullServicesCore();

    private static IServiceCollection AddPlasterSkullServicesCore(this IServiceCollection services) =>
        services
            .AddPlasterSkullInfrastructureServices()
            .AddPlasterSkullContextMenuServices();
}
