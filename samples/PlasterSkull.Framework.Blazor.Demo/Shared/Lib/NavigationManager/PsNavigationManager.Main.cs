namespace PlasterSkull.Framework.Blazor.Demo.Shared.Lib.NavigationManager;

partial class PsNavigationManager
{
    public static class MainRoutes
    {
        public const string Home = "/";
    }

    public static class ExternalRoutes
    {
        public const string GitHub = "https://github.com/PlasterSkull/Framework";
    }

    public void NavigateToHome() =>
        Instance.NavigateTo(MainRoutes.Home);

    public void NavigateToGitHub() =>
        Instance.NavigateTo(ExternalRoutes.GitHub);
}
