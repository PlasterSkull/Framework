using Microsoft.JSInterop;

namespace PlasterSkull.Framework.Blazor.Demo.Shared.Lib.NavigationManager;

public partial class PsNavigationManager(
    BlazorNavigationManager _navigationManager,
    IJSRuntime _jsRuntime)
{
    #region Public

    public BlazorNavigationManager Instance =>
        _navigationManager;

    public string RelativeUri =>
        ToRelativeUri(Instance.Uri);

    public string ToRelativeUri(string uri) =>
        uri.Replace(Instance.BaseUri, null);

    public ValueTask ChangeHistoryWithoutNavigate(string uri) =>
        _jsRuntime.InvokeVoidAsync("eval", $"window.history.pushState({{}}, '', '{uri}')");

    #endregion
}
