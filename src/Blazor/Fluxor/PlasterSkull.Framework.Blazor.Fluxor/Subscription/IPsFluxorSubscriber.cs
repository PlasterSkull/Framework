namespace PlasterSkull.Framework.Blazor.Fluxor;

public interface IPsFluxorSubscriber
{
    public TagId TagId { get; }
}

public interface IPsFluxorComponentSubscriber : IPsFluxorSubscriber
{
    public BlazorDispatcher GetBlazorDispatcher();
    public void CallStateHasChanged();
}