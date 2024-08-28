
namespace PlasterSkull.Framework.Blazor;

public interface IPsContextMenuProvider
{
    ValueTask CloseAllMenusAsync();
    ValueTask CloseMenuAsync(TagId menuId);
    Task<PsContextMenuInstanceObserver> ShowMenuAsync(PsContextMenuOpenArgs args);
}
