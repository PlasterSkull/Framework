
using ActualLab;
using MudBlazor;
using PlasterSkull.Framework.Blazor.Demo.Shared.Lib.CodeExampleCacheService;

namespace PlasterSkull.Framework.Blazor.Demo.Shared;

public partial class ExampleContainer : PsComponentBase
{
    #region Params

    [Parameter] public RenderFragment? ExampleInfoContent { get; set; }
    [Parameter] public RenderFragment? DemoContent { get; set; }
    [Parameter] public string? CodeSource { get; set; }

    #endregion

    #region Injects

    [Inject] ICodeExampleCacheService _codeExampleCacheService { get; init; } = null!;

    #endregion

    #region UI Fields

    private MarkupString _markupString;
    
    private bool _isCodeVisible;
    private string _codeVisibleSwitchButtonText =>
        _isCodeVisible
            ? "Hide code"
            : "Show code";
    private string _codeVisibleSwitchButtonIcon =>
        _isCodeVisible
            ? Icons.Material.Filled.CodeOff
            : Icons.Material.Filled.Code;

    #endregion

    #region LC Methods

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (CodeSource.IsNullOrEmpty())
            return;

        _markupString = await _codeExampleCacheService.GetAsync(CodeSource + ".razor.md");
    }

    #endregion
}
