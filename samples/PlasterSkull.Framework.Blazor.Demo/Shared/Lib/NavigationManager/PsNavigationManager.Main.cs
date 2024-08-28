namespace PlasterSkull.Framework.Blazor.Demo.Shared.Lib.NavigationManager;

partial class PsNavigationManager
{
    public class MainRoutes
    {
        public const string Home = "/";
    }

    public class ExternalRoutes
    {
        public const string GitHub = "https://github.com/PlasterSkull/Framework";
    }

    public void NavigateToHome() =>
        Instance.NavigateTo(MainRoutes.Home);

    public void NavigateToGitHub() =>
        Instance.NavigateTo(ExternalRoutes.GitHub);
}
