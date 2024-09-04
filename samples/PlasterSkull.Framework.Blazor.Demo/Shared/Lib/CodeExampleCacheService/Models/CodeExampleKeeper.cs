namespace PlasterSkull.Framework.Blazor.Demo.Shared.Lib.CodeExampleCacheService.Models;

internal class CodeExampleKeeper
{
    public MarkupString MarkupString { get; internal set; }
    public SemaphoreSlim Locker { get; } = new(1, 1);
}
