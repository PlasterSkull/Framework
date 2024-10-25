namespace PlasterSkull.Framework.Blazor;

public record PsRenderTracerOptions
{
    public string? Title { get; init; }
    public string? RenderInfoHexColor { get; init; } = MudExt.GenerateRandomMudColor().ToString(MudColorOutputFormats.HexA);
    public int ZIndex { get; init; } = 10;
    public Origin Origin { get; init; } = Origin.TopRight;
    public string? FontSize { get; init; }
    public string? Margin { get; init; }

    public static PsRenderTracerOptions CheckValues(PsRenderTracerOptions? renderInfoSettings) =>
        (renderInfoSettings ??= new()) with
        {
            RenderInfoHexColor = !string.IsNullOrEmpty(renderInfoSettings.RenderInfoHexColor)
                ? renderInfoSettings.RenderInfoHexColor
                : MudExt.GenerateRandomMudColor().ToString(MudColorOutputFormats.HexA),
            ZIndex = renderInfoSettings.ZIndex,
            Origin = renderInfoSettings.Origin,
            FontSize = !string.IsNullOrEmpty(renderInfoSettings.FontSize)
                ? renderInfoSettings.FontSize
                : "9px",
            Margin = renderInfoSettings.Margin,
        };
}
