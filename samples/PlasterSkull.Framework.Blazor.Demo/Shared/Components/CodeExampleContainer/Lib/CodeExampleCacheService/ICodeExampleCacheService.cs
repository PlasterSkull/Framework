namespace PlasterSkull.Framework.Blazor.Demo.Shared;

public interface ICodeExampleCacheService
{
    ValueTask<MarkupString> GetAsync(CodeExampleKey key, CancellationToken ct = default);
}
