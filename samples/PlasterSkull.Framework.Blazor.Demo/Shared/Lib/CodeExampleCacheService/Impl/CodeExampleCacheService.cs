using Markdig;
using Markdown.ColorCode;
using PlasterSkull.Framework.Blazor.Demo.Shared.Lib.CodeExampleCacheService.Models;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Reflection;

namespace PlasterSkull.Framework.Blazor.Demo.Shared.Lib.CodeExampleCacheService.Impl;

internal sealed class CodeExampleCacheService : ICodeExampleCacheService
{
    #region Injects

    private readonly ILogger<CodeExampleCacheService> _logger;

    #endregion

    #region Ctors

    static CodeExampleCacheService()
    {
        s_resourceFilesMap = Assembly.GetExecutingAssembly()
            .GetManifestResourceNames()
            .Where(f => f.EndsWith(".razor.md"))
            .ToFrozenDictionary(
                f => string.Join('.', f.Split('.').TakeLast(3)),
                f => f);

        s_sharedPipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseColorCode()
            .Build();
    }

    public CodeExampleCacheService(ILogger<CodeExampleCacheService> logger)
    {
        _logger = logger;
    }

    #endregion

    #region Fields

    private static readonly FrozenDictionary<string, string> s_resourceFilesMap;
    private static readonly MarkdownPipeline s_sharedPipeline;

    private readonly ConcurrentDictionary<string, CodeExampleKeeper> _codeExampleKeepers = new();

    #endregion

    #region Public

    public async ValueTask<MarkupString> GetAsync(string key, CancellationToken ct = default)
    {
        if (_codeExampleKeepers.TryGetValue(key, out var codeExampleKeeper))
        {
            await codeExampleKeeper.Locker.WaitAsync(ct);
            codeExampleKeeper.Locker.Release();

            return codeExampleKeeper.MarkupString;
        }

        var newCodeExampleKeeper = new CodeExampleKeeper();
        _ = _codeExampleKeepers.TryAdd(key, newCodeExampleKeeper);

        try
        {
            await newCodeExampleKeeper.Locker.WaitAsync(ct);

            if (!s_resourceFilesMap.TryGetValue(key, out var resourceFileName))
                return default;

            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceFileName);
            if (stream == null)
                return default;

            using var reader = new StreamReader(stream);
            return newCodeExampleKeeper.MarkupString = new(Markdig.Markdown.ToHtml(
                await reader.ReadToEndAsync(ct),
                s_sharedPipeline));
        }
        catch(Exception ex) 
        {
            _logger.LogError(ex, "Error while trying getting markup string");
        }
        finally
        {
            newCodeExampleKeeper.Locker.Release();
        }

        return default;
    }

    #endregion
}
