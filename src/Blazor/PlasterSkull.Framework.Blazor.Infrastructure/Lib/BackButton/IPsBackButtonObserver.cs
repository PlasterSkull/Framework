namespace PlasterSkull.Framework.Blazor;

public interface IPsBackButtonObserver
{
    TagId TagId { get; }

    PsBackButtonObserverInfo BackButtonObserverInfo { get; }

    ValueTask NotifyBackButtonEventAsync(PsBackButtonEventContext context, CancellationToken ct = default);
}
