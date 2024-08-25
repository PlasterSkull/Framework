namespace PlasterSkull.Framework.Blazor;

public record PsContextMenuOpenArgs
{
    public required Guid MenuId { get; init; }

    public required double X { get; init; }
    public required double Y { get; init; }
    public PsContextMenuSettings Settings { get; init; }

    public RenderFragment? RenderFragment { get; init; }

    public Func<Task>? OnHiding { get; init; }
}
