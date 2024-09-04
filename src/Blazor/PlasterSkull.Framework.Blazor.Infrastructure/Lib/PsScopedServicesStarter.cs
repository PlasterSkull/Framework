namespace PlasterSkull.Framework.Blazor;

public sealed class PsScopedServicesStarter(
    IMudDialogServiceListener _psDialogServiceListener)
{
    private readonly Tracer _tracer = Tracer.Default;

    public Task StartScopedServices() =>
        Task.Run(
            () =>
            {
                try
                {
                    _psDialogServiceListener.Start();
                }
                catch (Exception e)
                {
                    _tracer.Point($"{nameof(StartScopedServices)} failed, error: " + e);
                }
            },
            CancellationToken.None);
}
