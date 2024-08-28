namespace PlasterSkull.Framework.Blazor.Demo.App;

public partial class App
{
    [Inject] PsScopedServicesStarter _psScopedServicesStarter { get; init; } = null!;

    protected override Task OnInitializedAsync() =>
        _psScopedServicesStarter.StartScopedServices();
}
