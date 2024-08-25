namespace PlasterSkull.Framework.Blazor;

internal sealed class PsContextMenuService : IPsContextMenuService
{

    public event Action<PsContextMenuOpenArgs>? OnShowRequest;
    public event Action<Guid>? OnHideRequest;
    public event Action<Guid>? OnRenderRequest;

    public void Show(PsContextMenuOpenArgs args) => OnShowRequest?.Invoke(args);
    public void Hide(Guid menuId) => OnHideRequest?.Invoke(menuId);
    public void Render(Guid menuId) => OnRenderRequest?.Invoke(menuId);
}
