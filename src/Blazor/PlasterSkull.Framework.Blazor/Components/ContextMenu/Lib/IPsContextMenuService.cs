namespace PlasterSkull.Framework.Blazor;

public interface IPsContextMenuService
{
    event Action<PsContextMenuOpenArgs>? OnShowRequest;
    event Action<Guid>? OnHideRequest;
    event Action<Guid>? OnRenderRequest;

    void Hide(Guid menuId);
    void Show(PsContextMenuOpenArgs args);
    void Render(Guid menuId);
}
