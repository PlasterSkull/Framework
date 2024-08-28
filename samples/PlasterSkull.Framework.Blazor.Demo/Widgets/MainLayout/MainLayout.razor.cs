namespace PlasterSkull.Framework.Blazor.Demo.Widgets.MainLayout;

public partial class MainLayout : LayoutComponentBase
{
    #region Fields

    private bool _drawerOpen = true;

    #endregion

    #region Internal events

    private void ToggleDrawer() =>
        _drawerOpen = !_drawerOpen;

    #endregion
}
