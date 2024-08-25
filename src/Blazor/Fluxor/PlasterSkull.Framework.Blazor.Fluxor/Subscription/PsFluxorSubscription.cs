namespace PlasterSkull.Framework.Blazor.Fluxor;

public class PsFluxorSubscription<TAction> : IPsFluxorSubscription
    where TAction : IPsAction
{
    #region Ctors

    public PsFluxorSubscription(
        IPsFluxorSubscriber subcriber,
        IActionSubscriber actionSubscriber)
    {
        _tagId = subcriber.TagId;
        actionSubscriber.SubscribeToAction(subcriber, delegate (ActionWrapper<TAction> actionWrapper) {

            if (!CheckAllConditions(actionWrapper))
                return;

            if (subcriber is not IPsFluxorComponentSubscriber componentSubscriber || !_mustRender)
            {
                ExecuteAllSyncHandlers(actionWrapper);
                _ = ExecuteAllAsyncHandlers(actionWrapper);
                return;
            }

            componentSubscriber
                .GetBlazorDispatcher()
                .InvokeAsync(async delegate {
                    ExecuteAllSyncHandlers(actionWrapper);
                    await ExecuteAllAsyncHandlers(actionWrapper);
                    componentSubscriber.CallStateHasChanged();
                });
        });
    }

    #endregion

    #region Fields

    private readonly TagId _tagId;
    private readonly List<Action<ActionWrapper<TAction>>> _syncHandlers = [];
    private readonly List<Func<ActionWrapper<TAction>, Task>> _asyncHandlers = [];
    private readonly List<Func<ActionWrapper<TAction>, bool>> _conditions = [];
    private bool _mustRender = true;

    #endregion

    #region Pipe methods

    public PsFluxorSubscription<TAction> WithHandler(Action<ActionWrapper<TAction>> syncHandler)
    {
        _syncHandlers.Add(syncHandler);
        return this;
    }

    public PsFluxorSubscription<TAction> WithHandler(Func<ActionWrapper<TAction>, Task> asyncHanlder)
    {
        _asyncHandlers.Add(asyncHanlder);
        return this;
    }

    public PsFluxorSubscription<TAction> WithCondition(Func<ActionWrapper<TAction>, bool> condition)
    {
        _conditions.Add(condition);
        return this;
    }

    public PsFluxorSubscription<TAction> WithTagReceiverCondition()
    {
        _conditions.Add(action => action.TagId == _tagId);
        return this;
    }

    public PsFluxorSubscription<TAction> WithoutRender()
    {
        _mustRender = false;
        return this;
    }

    #endregion

    #region Private methods

    private bool CheckAllConditions(ActionWrapper<TAction> actionWrapper) =>
        _conditions.All(condition => condition.Invoke(actionWrapper));

    private void ExecuteAllSyncHandlers(ActionWrapper<TAction> actionWrapper) =>
        _syncHandlers.ForEach(syncHandler => syncHandler.Invoke(actionWrapper));

    private Task ExecuteAllAsyncHandlers(ActionWrapper<TAction> actionWrapper) =>
        Task.WhenAll(_asyncHandlers.Select(asyncHandler => asyncHandler.Invoke(actionWrapper)));

    #endregion
}
