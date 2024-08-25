namespace PlasterSkull.Framework.Blazor.Fluxor;

public abstract class PsErrorOrEffectBase<TAction> : PsEffectBase<TAction>
    where TAction : IPsAction
{
    #region Ctors

    protected PsErrorOrEffectBase(IServiceProvider services) : base(services) { }

    #endregion

    protected override Task PsHandleAsync(ActionWrapper<TAction> actionWrapper) => PsHandleAsync(actionWrapper.ToErrorOr());

    protected abstract Task PsHandleAsync(ErrorOr<ActionWrapper<TAction>> actionWrapper);
}