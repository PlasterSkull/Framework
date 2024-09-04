namespace PlasterSkull.Framework.Blazor;

public interface IMudThemeService
{
    MudTheme Current { get; }
    bool IsDarkMode { get; }

    Palette Palette { get; }

    void Set(MudTheme theme, bool isDark);
}
