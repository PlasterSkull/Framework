namespace PlasterSkull.Framework.Blazor;

public record PsRenderTracerOptions
{
    public string? RenderInfoHexColor { get; init; } = MudExt.GenerateRandomMudColor().ToString(MudColorOutputFormats.HexA);
    public int ZIndex { get; init; } = 10;
    public Origin RenderInfoOrigin { get; init; } = Origin.TopRight;
    public string? RenderInfoFontSize { get; init; }
    public string? RenderInfoMargin { get; init; }

    public static PsRenderTracerOptions CheckValues(PsRenderTracerOptions? renderInfoSettings) =>
        (renderInfoSettings ??= new()) with
        {
           RenderInfoHexColor = !string.IsNullOrEmpty(renderInfoSettings.RenderInfoHexColor)
               ? renderInfoSettings.RenderInfoHexColor
               : MudExt.GenerateRandomMudColor().ToString(MudColorOutputFormats.HexA),
           ZIndex = renderInfoSettings.ZIndex,
           RenderInfoOrigin = renderInfoSettings.RenderInfoOrigin,
           RenderInfoFontSize = !string.IsNullOrEmpty(renderInfoSettings.RenderInfoFontSize)
               ? renderInfoSettings.RenderInfoFontSize
               : "9px",
           RenderInfoMargin = renderInfoSettings.RenderInfoMargin,
        };   
}
