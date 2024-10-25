
namespace PlasterSkull.Framework.Blazor.ServerSide;

internal sealed class ServerSideBackButtonHandler : IPsBackButtonClickNativeHandler
{
    public ValueTask HandleAsync(PsBackButtonEventContext context, CancellationToken ct = default) =>
        ValueTask.CompletedTask;
}
