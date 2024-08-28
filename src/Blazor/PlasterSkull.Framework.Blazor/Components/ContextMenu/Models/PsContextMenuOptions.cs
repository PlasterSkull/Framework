using ActualLab;

namespace PlasterSkull.Framework.Blazor;

public readonly struct PsContextMenuOptions
{
    public required TagId CallerId { get; init; }
    public required double X { get; init; }
    public required double Y { get; init; }
    internal int ZIndex { get; init; }
    public string? Title { get; init; }
    public bool FullScreenMobile { get; init; }
    public bool ShowMobileHeader { get; init; }
    public Origin Origin { get; init; }
}
