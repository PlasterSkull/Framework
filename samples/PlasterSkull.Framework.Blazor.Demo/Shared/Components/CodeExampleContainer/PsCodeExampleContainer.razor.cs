using MudBlazor.Utilities;
using System.Collections.Immutable;

namespace PlasterSkull.Framework.Blazor.Demo.Shared;

public partial class PsCodeExampleContainer : PsComponentBase
{
    #region Params

    [Parameter, EditorRequired] public required string Title { get; set; }
    [Parameter, EditorRequired] public required IEnumerable<CodeExampleKey> CodeSourcesKeys { get; set; }

    [Parameter] public RenderFragment? ExampleInfoContent { get; set; }
    [Parameter] public RenderFragment? DemoContent { get; set; }

    #endregion

    #region Injects

    [Inject] ICodeExampleCacheService _codeExampleCacheService { get; init; } = null!;

    #endregion

    #region Fields

    private MudTabs _mudTabsRef = null!;

    private ImmutableList<CodeExample> _codeExamplesMap = [];

    private bool _isCodeVisible;
    private string _codeVisibleSwitchButtonText =>
        _isCodeVisible
            ? "Hide code"
            : "Show code";

    private string _codeVisibleSwitchButtonIcon =>
        _isCodeVisible
            ? Icons.Material.Filled.CodeOff
            : Icons.Material.Filled.Code;

    private Variant GetCodeExampleTabButtonVariant(CodeExampleKey codeExampleKey) =>
        (CodeExampleKey)_mudTabsRef.ActivePanel.ID == codeExampleKey
            ? Variant.Filled
            : Variant.Text;

    #endregion

    #region Css/Style

    private string InnerContainerStyleName =>
        new StyleBuilder()
            .AddStyle("border", _borderStyle)
            .Build();

    private string CodeContainerStyleName =>
        new StyleBuilder()
            .AddStyle("max-height", "25vh")
            .AddStyle("border-top", _borderStyle)
            .Build();

    private string _borderStyle =>
        $"1px solid {_mudThemeService.Palette.LinesDefault}";

    #endregion

    #region LC Events

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var codeExamplesLoadTasks = CodeSourcesKeys
            .Select(key =>
                (Key: key,
                GetTask: _codeExampleCacheService.GetAsync(key).AsTask()))
            .ToList();

        await Task.WhenAll(codeExamplesLoadTasks.Select(x => x.GetTask));

        _codeExamplesMap = codeExamplesLoadTasks
            .Select(c => new CodeExample
            {
                Key = c.Key,
                Content = c.GetTask.Result
            })
            .ToImmutableList();
    }

    #endregion

    private void OnCodeExampleButtonClick(CodeExampleKey codeExampleKey) =>
        _mudTabsRef.ActivatePanel(codeExampleKey);

    #region Nested models

    private readonly struct CodeExample
    {
        public required CodeExampleKey Key { get; init; }
        public required MarkupString Content { get; init; }
    }

    #endregion
}
