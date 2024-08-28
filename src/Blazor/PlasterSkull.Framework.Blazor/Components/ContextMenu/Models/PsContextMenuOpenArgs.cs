namespace PlasterSkull.Framework.Blazor;

public record PsContextMenuOpenArgs
{
    public required PsContextMenuOptions Options { get; init; }

    public required RenderFragment MenuContent { get; init; }
}
