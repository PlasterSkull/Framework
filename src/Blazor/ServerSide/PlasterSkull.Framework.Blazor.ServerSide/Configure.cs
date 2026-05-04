using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PlasterSkull.Framework.Blazor.ServerSide;

public static class Configure
{
    public static IHostApplicationBuilder UsePlasterSkullServerSideServices(this IHostApplicationBuilder builder)
    {
        var hostSettings = builder.Configuration.Get<HostSettings>() ??
            throw new Exception($"No configuration for {nameof(HostSettings)}");

        builder.Services.AddSingleton(s => new HostInfo
        {
            AppKind = AppKind.WebServer,
            ClientKind = ClientKind.Unknown,
            Environment = builder.Environment.EnvironmentName,
            BaseUrl = hostSettings.BaseUri,
        });

        builder.Services.AddSingleton<IPsBackButtonClickNativeHandler, ServerSideBackButtonHandler>();

        return builder;
    }
}
