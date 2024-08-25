namespace PlasterSkull.Framework.Blazor;

public partial class PsIcon : PsComponentBase
{
    #region Params

    [Parameter] public string Icon { get; set; } = Icons.Material.Filled.Circle;
    [Parameter] public IconType IconType { get; set; } = IconType.Default;
    [Parameter] public string Title { get; set; } = string.Empty;

    [Parameter] public Color Color { get; set; } = Color.Primary;
    [Parameter] public string? HexColor { get; set; }
    [Parameter] public string? FillHexColor { get; set; }
    [Parameter] public string? FillHoverHexColor { get; set; }

    [Parameter] public string SizeClass { get; set; } = "icon-24";

    [Parameter] public string Width { get; set; } = string.Empty;
    [Parameter] public string Height { get; set; } = string.Empty;
    [Parameter] public string CircleButtonSize { get; set; } = string.Empty;
    [Parameter] public string ViewBox { get; set; } = "0 0 24 24";
    [Parameter] public bool ShowLeftBorder { get; set; } = true;

    [Parameter] public Variant ButtonVariant { get; set; }
    [Parameter] public ButtonType ButtonType { get; set; }
    [Parameter] public bool ButtonDisabled { get; set; }
    [Parameter] public bool ButtonDropShadow { get; set; }
    [Parameter] public bool ButtonRipple { get; set; } = true;
    [Parameter] public bool ButtonClickPropagation { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnButtonClick { get; set; }

    [Parameter] public string InnerIconClass { get; set; } = string.Empty;
    [Parameter] public string InnerIconStyle { get; set; } = string.Empty;

    #endregion

    #region UI Fields

    private Color _color => 
        string.IsNullOrEmpty(HexColor) 
            ? Color 
            : Color.Inherit;

    #endregion

    #region Css/Style

    private string _typeClass =>
        $"{RootClassName}-{IconType.ToString().ToKebabCase()}";

    protected override CssBuilder? ExtendClassNameBuilder =>
        new CssBuilder()
            .AddClass($"{_typeClass}")
            .AddClass($"{_typeClass}-variant-{ButtonVariant.ToString().ToKebabCase()}", IconType is IconType.CircleButton)
            .AddClass($"{SizeClass}", !string.IsNullOrEmpty(SizeClass))
            .AddClass($"ps-icon-with-fill-hex-color", !string.IsNullOrEmpty(FillHexColor))
            .AddClass($"ps-icon-with-fill-hover-hex-color", !string.IsNullOrEmpty(FillHoverHexColor));

    protected override StyleBuilder? ExtendStyleNameBuilder =>
        new StyleBuilder()
            .AddStyle("color", HexColor, !string.IsNullOrEmpty(HexColor));

    private string CircleClassName =>
        new CssBuilder()
            .When(IconType is IconType.Circle, circleBuilder => circleBuilder
                .AddClass("border", ShowLeftBorder)
                .AddClass("border-t border-r border-b", !ShowLeftBorder)
                .AddClass("rounded-circle h-min-content")
                .AddClass($"mud-border-{Color.ToString().ToLower()}", string.IsNullOrEmpty(HexColor)))
            .When(IconType is IconType.CircleButton, circleButtonBuilder => circleButtonBuilder
                .AddClass("rounded-circle h-min-content"))
            .AddClass(SizeClass, !string.IsNullOrEmpty(SizeClass))
            .AddClass(InnerIconClass)

            .Build();

    private string CircleStyleName =>
        new StyleBuilder()
            .AddStyle("border-color", HexColor, !string.IsNullOrEmpty(HexColor))
            .AddStyle("width", CircleButtonSize, !string.IsNullOrEmpty(CircleButtonSize))
            .AddStyle("height", CircleButtonSize, !string.IsNullOrEmpty(CircleButtonSize))
            .AddStyle("box-shadow", "none!important")
            .AddStyle("-webkit-box-shadow", "none !important")
            .AddStyle(InnerIconStyle)
            .Build();

    private string InnerIconClassName =>
        new CssBuilder("inner-icon")
            .AddClass(InnerIconClass)
            .Build();

    private string InnerIconStyleName =>
        new StyleBuilder()
            .AddStyle("width", Width, !string.IsNullOrEmpty(Width))
            .AddStyle("height", Height, !string.IsNullOrEmpty(Height))
            .AddStyle("--fill-hex-color", FillHexColor, !string.IsNullOrEmpty(FillHexColor))
            .AddStyle("--fill-hover-hex-color", FillHoverHexColor, !string.IsNullOrEmpty(FillHoverHexColor))
            .AddStyle(InnerIconStyle)
            .Build();

    #endregion
}
