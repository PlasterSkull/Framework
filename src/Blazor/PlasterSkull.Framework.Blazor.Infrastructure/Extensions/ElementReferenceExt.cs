
using Microsoft.JSInterop;
using System.Reflection;

namespace PlasterSkull.Framework.Blazor;

public static class ElementReferenceExt
{
    private static readonly PropertyInfo? jsRuntimeProperty =
        typeof(WebElementReferenceContext).GetProperty("JSRuntime", BindingFlags.Instance | BindingFlags.NonPublic);

    internal static IJSRuntime? GetJSRuntime(this ElementReference elementReference)
    {
        if (elementReference.Context is not WebElementReferenceContext context)
        {
            return null;
        }

        return (IJSRuntime?)jsRuntimeProperty?.GetValue(context);
    }

    public static ValueTask<T?> GetElementPropertyValue<T>(
        this ElementReference elementReference,
        string propertyName) =>
        elementReference
            .GetJSRuntime()?
            .InvokeAsync<T?>("window.ElementReference.getElementPropertyValue", elementReference, propertyName) ??
        ValueTask.FromResult(default(T));

    public static ValueTask<T?> GetElementPropertyValue<T>(
        this ElementReference? elementReference,
        string propertyName) =>
        elementReference.HasValue
            ? elementReference.Value
                .GetJSRuntime()?
                .InvokeAsync<T?>(
                    "window.ElementReference.getElementPropertyValue",
                    elementReference,
                    propertyName) ??
                ValueTask.FromResult(default(T))
            : ValueTask.FromResult(default(T));

    public static ValueTask DisableMousewheelScroll(this ElementReference elementReference) =>
        elementReference
            .GetJSRuntime()?
            .InvokeVoidAsync("window.ElementReference.disableMousewheelScroll", elementReference) ??
        ValueTask.CompletedTask;
}
