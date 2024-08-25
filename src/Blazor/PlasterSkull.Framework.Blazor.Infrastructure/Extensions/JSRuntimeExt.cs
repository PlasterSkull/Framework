using Microsoft.JSInterop;

namespace PlasterSkull.Framework.Blazor;

public static class JSRuntimeExt
{
    public static async Task CopyToClipboard(this IJSRuntime jsRuntime, string value)
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("eval", $"window.navigator.clipboard.writeText('{value}')");
        }
        catch (Exception)
        {
            //Потеря фокуса (Альт-таб) во время выполнения приводит к ошибке
        }
    }

    public static ValueTask EvalVoid(this IJSRuntime js, string expression, CancellationToken cancellationToken = default) =>
        js.InvokeVoidAsync("eval", cancellationToken, expression);

    public static ValueTask<T> Eval<T>(this IJSRuntime js, string expression, CancellationToken cancellationToken = default) =>
        js.InvokeAsync<T>("eval", cancellationToken, expression);
}
