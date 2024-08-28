namespace PlasterSkull.Framework.Blazor.Demo.Pages;

[Route(PsNavigationManager.MainRoutes.Home)]
public partial class HomePage : PsComponentBase
{
    #region Injects

    [Inject] PsNavigationManager _psMavigationManager { get; init; } = null!;

    #endregion
}
