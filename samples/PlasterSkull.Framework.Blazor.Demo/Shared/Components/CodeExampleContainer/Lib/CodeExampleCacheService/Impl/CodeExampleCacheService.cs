using Markdig;
using Markdown.ColorCode;
using System.Collections.Frozen;
using System.Reflection;

namespace PlasterSkull.Framework.Blazor.Demo.Shared;

internal sealed class CodeExampleCacheService : ICodeExampleCacheService
{
    #region Injects

    private readonly ILogger<CodeExampleCacheService> _logger;
    private readonly IMudThemeService _mudThemeService;

    #endregion

    #region Ctors

    static CodeExampleCacheService()
    {
        _codeExampleMap = Assembly.GetExecutingAssembly()
            .GetManifestResourceNames()
            .Where(f => f.EndsWith(".razor.md"))
            .Select(f => (
                Key: new CodeExampleKey(f.Split('.').TakeLast(3).ElementAt(0)),
                ResourcePath: f))
            .ToFrozenDictionary(
                f => f.Key,
                f => new CodeExampleKeeper
                {
                    Key = f.Key,
                    ResourcePath = f.ResourcePath,
                });
    }

    public CodeExampleCacheService(
        ILogger<CodeExampleCacheService> logger,
        IMudThemeService mudThemeService)
    {
        _logger = logger;
        _mudThemeService = mudThemeService;

        _sharedPipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseColorCode(defaultLanguageId: "c#")
            .Build();
    }

    #endregion

    #region Fields

    private static readonly FrozenDictionary<CodeExampleKey, CodeExampleKeeper> _codeExampleMap;

    private readonly MarkdownPipeline _sharedPipeline;

    #endregion

    #region Public

    public async ValueTask<MarkupString> GetAsync(CodeExampleKey key, CancellationToken ct = default)
    {
        if (!_codeExampleMap.TryGetValue(key, out var codeExampleKeeper))
        {
            LogMissedResource();
            return default;
        }

        await codeExampleKeeper.Locker.WaitAsync(ct);

        if (codeExampleKeeper.IsLoadingTriggered)
        {
            codeExampleKeeper.Locker.Release();

            return codeExampleKeeper.MarkupString;
        }

        codeExampleKeeper.IsLoadingTriggered = true;

        try
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(codeExampleKeeper.ResourcePath);
            if (stream == null)
            {
                LogMissedResource();
                return default;
            }

            using var reader = new StreamReader(stream);
            return codeExampleKeeper.MarkupString = new(Markdig.Markdown.ToHtml(
                await reader.ReadToEndAsync(ct),
                _sharedPipeline));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while trying getting markup string");
        }
        finally
        {
            codeExampleKeeper.Locker.Release();
        }

        return default;

        void LogMissedResource() =>
            _logger.LogError("Resource missed: {Key}", key);
    }

    #endregion
}
