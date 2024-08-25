namespace PlasterSkull.Framework.Blazor;

public interface IPsBackButtonService
{
    ValueTask<PsBackButtonEventContext> PushAsync(CancellationToken ct = default);
    void Subscribe(IPsBackButtonObserver subscriber);
    void Unsubscribe(IPsBackButtonObserver subscriber);
    void Unsubscribe(TagId tagId);
}
