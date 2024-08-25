namespace PlasterSkull.Framework.Blazor;

public partial class PsContextMenuMoreButton : PsContextMenuTriggerComponentBase
{
    #region Params

    [Parameter] public string IconSize { get; set; } = "icon-16";
    [Parameter] public string HexColor { get; set; } = "green";//Themes.DefaultTheme.Palette.Primary.ToString(MudColorOutputFormats.Hex);

    #endregion
}
