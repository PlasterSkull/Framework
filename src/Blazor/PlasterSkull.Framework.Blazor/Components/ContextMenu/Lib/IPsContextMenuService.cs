
namespace PlasterSkull.Framework.Blazor;

public interface IPsContextMenuService
{
    void SetupProvider(IPsContextMenuProvider psContextMenuProvider);
    Task<PsContextMenuInstanceObserver> ShowMenuAsync(PsContextMenuOpenArgs args);
    ValueTask CloseMenuAsync(TagId callerId);
    ValueTask CloseAllMenusAsync();
}
