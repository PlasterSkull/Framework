namespace PlasterSkull.Framework.Blazor;

internal sealed class PsContextMenuInstanceObserver
{
    public required Guid Id { get; init; }

    public required double X { get; init; }
    public required double Y { get; init; }

    public required PsContextMenuSettings Settings { get; init; }

    public RenderFragment? MenuContent { get; init; }
    public PsContextMenuInstance? Instance { get; private set; }

    public Func<Task>? OnHiding { get; init; }

    public void SetInstance(PsContextMenuInstance? instance) => Instance = instance;
}
