namespace PlasterSkull.Framework.Blazor.Demo.Shared;

internal sealed class CodeExampleKeeper
{
    public required CodeExampleKey Key { get; set; }
    public MarkupString MarkupString { get; set; }

    public required string ResourcePath { get; set; }
    public bool IsLoadingTriggered { get; set; }
    public SemaphoreSlim Locker { get; } = new(1, 1);
}
