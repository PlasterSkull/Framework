namespace PlasterSkull.Framework.Blazor;

public interface IPsBackButtonClickNativeHandler
{
    ValueTask HandleAsync(PsBackButtonEventContext context, CancellationToken ct = default);
}
