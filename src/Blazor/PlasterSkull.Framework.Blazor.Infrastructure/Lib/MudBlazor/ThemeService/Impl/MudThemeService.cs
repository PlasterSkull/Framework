
namespace PlasterSkull.Framework.Blazor;

internal sealed class MudThemeService : IMudThemeService
{
    private MudTheme _current = new();
    private bool _isDark;

    public MudTheme Current => _current;
    public bool IsDarkMode => _isDark;

    public Palette Palette => IsDarkMode
        ? Current.PaletteDark
        : Current.PaletteLight;

    public void Set(
        MudTheme theme,
        bool isDark)
    {
        _current = theme;
        _isDark = isDark;
    }
}
