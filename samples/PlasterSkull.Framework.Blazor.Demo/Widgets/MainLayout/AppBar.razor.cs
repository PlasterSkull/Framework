namespace PlasterSkull.Framework.Blazor.Demo.Widgets.MainLayout;

public partial class AppBar : PsComponentBase
{
    #region Params

    [Parameter] public EventCallback OnDrawerTogglerClick { get; set; }

    #endregion
}
