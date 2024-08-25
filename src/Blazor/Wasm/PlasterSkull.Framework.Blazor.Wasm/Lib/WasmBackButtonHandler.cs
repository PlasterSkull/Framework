
namespace PlasterSkull.Framework.Blazor.Wasm;

internal sealed class WasmBackButtonHandler : IPsBackButtonClickNativeHandler
{
    public ValueTask HandleAsync(PsBackButtonEventContext context, CancellationToken ct = default) => 
        ValueTask.CompletedTask;
}
