using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace PlasterSkull.Framework.Blazor.Wasm;

public static class Configure
{
    public static WebAssemblyHostBuilder UsePlasterSkullWasmServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddSingleton(s => new HostInfo
        {
            AppKind = AppKind.WasmApp,
            ClientKind = ClientKind.Wasm,
            Environment = builder.HostEnvironment.Environment,
            BaseUrl = builder.HostEnvironment.BaseAddress,
        });

        builder.Services.AddSingleton<IPsBackButtonClickNativeHandler, WasmBackButtonHandler>();

        return builder;
    }
}
