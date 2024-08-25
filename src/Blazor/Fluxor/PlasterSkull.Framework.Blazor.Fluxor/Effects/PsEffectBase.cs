using ActualLab.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PlasterSkull.Framework.Blazor.Fluxor;

public abstract class PsEffectBase<TAction> : Effect<ActionWrapper<TAction>>
    where TAction : IPsAction
{
    #region Injects

    protected readonly IServiceProvider _services;

    #endregion

    #region Ctors

    protected PsEffectBase(IServiceProvider services)
    {
        _services = services;
    }

    #endregion

    #region Services 

    private ILogger? c_logger;
    protected ILogger _logger =>
        c_logger ??= _services.LogFor(GetType());

    #endregion

    #region Fields

    protected IDispatcher Dispatcher = null!;
    protected TagId? CallerId;
    protected CancellationToken CancellationToken;

    #endregion

    #region Public/Protected

    public override async Task HandleAsync(ActionWrapper<TAction> actionWrapper, IDispatcher dispatcher)
    {
        Dispatcher = dispatcher;
        CallerId = actionWrapper.TagId;
        CancellationToken = actionWrapper.CancellationToken;
        var tracerRegion = Tracer.Default.Region($"{GetType().Name} | {Guid.NewGuid()}", true);
        await PsHandleAsync(actionWrapper);
        tracerRegion.Close();
    }

    protected void PsDispatch<TNextAction>(TNextAction nextAction) where TNextAction : IPsAction =>
        Dispatcher.Dispatch(new ActionWrapper<TNextAction>
        {
            Action = nextAction,
            TagId = CallerId,
            CancellationToken = CancellationToken,
        });

    protected abstract Task PsHandleAsync(ActionWrapper<TAction> actionWrapper);

    #endregion
}