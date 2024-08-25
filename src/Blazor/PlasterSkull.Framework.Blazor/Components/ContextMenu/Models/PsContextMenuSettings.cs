namespace PlasterSkull.Framework.Blazor;

public readonly struct PsContextMenuSettings
{
    public string? Title { get; init; }
    public bool FullScreenMobile { get; init; }
    public bool ShowMobileHeader { get; init; }
    public Origin Origin { get; init; }
}
