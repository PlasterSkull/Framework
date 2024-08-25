namespace PlasterSkull.Framework.Blazor.Fluxor;

internal class PsFluxorActionResolver(
    IActionSubscriber _actionSubscriber,
    IDispatcher _dispatcher)
    : IDisposable
{
    #region Fields

    private readonly List<IPsFluxorSubscription> _subscriptions = [];
    private IPsFluxorSubscriber? _subscriber;

    #endregion

    #region Public

    public void SetSubscriber(IPsFluxorSubscriber subscriber) =>
        _subscriber = subscriber;

    public PsFluxorSubscription<TAction> SubscribeTo<TAction>() where TAction : IPsAction
    {
        var subscription = new PsFluxorSubscription<TAction>(
            EnsureSubscriberExist(),
            _actionSubscriber);

        _subscriptions.Add(subscription);
        return subscription;
    }

    public void Dispatch<TAction>(TAction action, CancellationToken ct = default) where TAction : IPsAction =>
        _dispatcher.Dispatch(new ActionWrapper<TAction>
        {
            Action = action,
            TagId = EnsureSubscriberExist().TagId,
            CancellationToken = ct,
        });

    public void Dispose()
    {
        _subscriptions.Clear();
        if (_subscriber != null)
        {
            _actionSubscriber.UnsubscribeFromAllActions(_subscriber);
            _subscriber = null;
        }
    }

    #endregion

    #region Private methods

    private IPsFluxorSubscriber EnsureSubscriberExist() =>
        _subscriber ?? throw new InvalidOperationException("Subscriber not exist");

    #endregion
}
