namespace PlasterSkull.Framework.Blazor;

public static class MudExt
{
    #region Color

    public static MudColor GenerateRandomMudColor()
    {
        byte[] randomColor = new byte[3];
        new Random().NextBytes(randomColor);
        return new MudColor(randomColor[0], randomColor[1], randomColor[2], byte.MaxValue);
    }

    public static string GetColorCssClass(this Color color) =>
        $"mud-{color.ToDescriptionString()}-text";

    public static string GetBackgroundColorCssClass(this Color color) =>
        $"mud-{color.ToDescriptionString()}";

    public static string ToHex(this MudColor color) =>
        color.ToString(MudColorOutputFormats.Hex);

    public static string ToHexA(this MudColor color) =>
        color.ToString(MudColorOutputFormats.HexA);

    #endregion

    #region Size

    public static string ToSizeIconClass(this Size size) =>
        size switch
        {
            Size.Small => "icon-16",
            Size.Large => "icon-32",
            Size.Medium or _ => "icon-24",
        };

    #endregion

    #region Position

    public static string AbsoluteLeftOffset(this Origin origin) =>
        origin switch
        {
            Origin.TopLeft => "0",
            Origin.CenterLeft => "0",
            Origin.BottomLeft => "0",
            Origin.TopCenter => "50%",
            Origin.BottomCenter => "50%",
            Origin.CenterCenter => "50%",
            _ => "",
        };

    public static string AbsoluteTopOffset(this Origin origin) =>
        origin switch
        {
            Origin.TopLeft => "0",
            Origin.TopCenter => "0",
            Origin.TopRight => "0",
            Origin.CenterLeft => "50%",
            Origin.CenterCenter => "50%",
            Origin.CenterRight => "50%",
            _ => "",
        };

    public static string AbsoluteRightOffset(this Origin origin) =>
        origin switch
        {
            Origin.TopRight => "0",
            Origin.CenterRight => "0",
            Origin.BottomRight => "0",
            _ => "",
        };

    public static string AbsoluteBottomOffset(this Origin origin) =>
        origin switch
        {
            Origin.BottomLeft => "0",
            Origin.BottomCenter => "0",
            Origin.BottomRight => "0",
            _ => "",
        };

    #endregion

    #region Css/Style Builder

    public static string WithImportantCssProperties(this string css) =>
        css.Replace(";", " !important;");

    public static string ImportantCssPropertyValue(this string value) =>
        $"{value} !important";

    public static string GetRootClassName(this Type type)
    {
        var result = type.Name.ToKebabCase();

        int apostropheIndex = result.IndexOf('`');
        if (apostropheIndex > 0)
            return result[..apostropheIndex];

        return result;
    }

    public static CssBuilder AddClass(
        this CssBuilder builder,
        Func<CssBuilder> other,
        bool when = true) =>
        when
        ? builder.AddClass(other().Build())
        : builder;

    public static CssBuilder AddClass(
        this CssBuilder builder,
        Func<CssBuilder, CssBuilder> other,
        bool when = true) =>
        when
        ? builder.AddClass(other(new CssBuilder()).Build())
        : builder;

    public static CssBuilder When(
        this CssBuilder builder,
        bool when,
        Func<CssBuilder> other) =>
        when
        ? builder.AddClass(other(), true)
        : builder;

    public static CssBuilder When(
        this CssBuilder builder,
        bool when,
        Func<CssBuilder, CssBuilder> other) =>
        when
        ? builder.AddClass(other(new CssBuilder()), true)
        : builder;

    public static CssBuilder When(
        this CssBuilder builder,
        Func<bool> whenFunc,
        Func<CssBuilder> other) =>
        whenFunc()
        ? builder.AddClass(other(), true)
        : builder;

    public static CssBuilder When(
        this CssBuilder builder,
        Func<bool> whenFunc,
        Func<CssBuilder, CssBuilder> other) =>
        whenFunc()
        ? builder.AddClass(other(new CssBuilder()), true)
        : builder;

    public static StyleBuilder AddStyle(
        this StyleBuilder builder,
        Func<StyleBuilder> other,
        bool when = true) =>
        when
        ? builder.AddStyle(other().Build())
        : builder;

    public static StyleBuilder AddStyle(
        this StyleBuilder builder,
        Func<StyleBuilder, StyleBuilder> other,
        bool when = true) =>
        when
        ? builder.AddStyle(other(new StyleBuilder()).Build())
        : builder;

    public static StyleBuilder When(
        this StyleBuilder builder,
        bool when,
        Func<StyleBuilder> other) =>
        when
        ? builder.AddStyle(other())
        : builder;

    public static StyleBuilder When(
        this StyleBuilder builder,
        bool when,
        Func<StyleBuilder, StyleBuilder> other) =>
        when
        ? builder.AddStyle(other(new StyleBuilder()))
        : builder;

    public static StyleBuilder When(
        this StyleBuilder builder,
        Func<bool> whenFunc,
        Func<StyleBuilder> other) =>
        whenFunc()
        ? builder.AddStyle(other())
        : builder;

    public static StyleBuilder When(
        this StyleBuilder builder,
        Func<bool> whenFunc,
        Func<StyleBuilder, StyleBuilder> other) =>
        whenFunc()
        ? builder.AddStyle(other(new StyleBuilder()))
        : builder;

    #endregion
}
