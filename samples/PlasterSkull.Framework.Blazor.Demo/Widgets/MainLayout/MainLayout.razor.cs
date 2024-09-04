using MudBlazor;

namespace PlasterSkull.Framework.Blazor.Demo.Widgets.MainLayout;

public partial class MainLayout : LayoutComponentBase
{
    #region Injects

    [Inject] IMudThemeService _mudThemeService { get; init; } = null!;

    #endregion

    #region Fields

    private bool _drawerOpen = true;

    #endregion

    #region LC Methods

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _mudThemeService.Set(new MudTheme(), true);
    }

    #endregion

    #region Internal events

    private void ToggleDrawer() =>
        _drawerOpen = !_drawerOpen;

    #endregion
}
