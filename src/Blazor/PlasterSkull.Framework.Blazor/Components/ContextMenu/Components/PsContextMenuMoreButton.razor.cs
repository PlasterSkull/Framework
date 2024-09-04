namespace PlasterSkull.Framework.Blazor;

public partial class PsContextMenuMoreButton : PsContextMenuTriggerComponentBase
{
    #region Params

    [Parameter] public string IconSize { get; set; } = "icon-16";
    [Parameter] public string? HexColor { get; set; }

    #endregion

    #region Fields

    private string? _hexColor;

    #endregion

    #region LC Methods

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _hexColor = HexColor ?? _mudThemeService.Palette.Primary.ToString(MudColorOutputFormats.Hex);
    }

    #endregion
}
