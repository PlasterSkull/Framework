using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PlasterSkull.Framework.Blazor;
using PlasterSkull.Framework.Blazor.Demo.App;
using PlasterSkull.Framework.Blazor.Wasm;

var builder = WebAssemblyHostBuilder
    .CreateDefault(args)
    .UsePlasterSkullWasmServices();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var services = builder.Services;

services.AddPlasterSkullServices();
services.ConfigureAppLayer();

await builder.Build().RunAsync();
