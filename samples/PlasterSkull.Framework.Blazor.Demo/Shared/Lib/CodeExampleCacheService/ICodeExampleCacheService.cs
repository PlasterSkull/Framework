
namespace PlasterSkull.Framework.Blazor.Demo.Shared.Lib.CodeExampleCacheService;

public interface ICodeExampleCacheService
{
    ValueTask<MarkupString> GetAsync(string key, CancellationToken ct = default);
}
