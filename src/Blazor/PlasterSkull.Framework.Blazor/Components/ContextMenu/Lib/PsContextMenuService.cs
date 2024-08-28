namespace PlasterSkull.Framework.Blazor;

internal sealed class PsContextMenuService : IPsContextMenuService
{
    #region Provider

    private IPsContextMenuProvider? _psContextMenuProvider;

    public void SetupProvider(IPsContextMenuProvider psContextMenuProvider) =>
        _psContextMenuProvider = psContextMenuProvider;

    private IPsContextMenuProvider EnsureProviderInitialized() =>
        _psContextMenuProvider ??
        throw new InvalidOperationException($"{nameof(IPsContextMenuProvider)} not initialized.");

    #endregion

    public Task<PsContextMenuInstanceObserver> ShowMenuAsync(PsContextMenuOpenArgs args) =>
        EnsureProviderInitialized().ShowMenuAsync(args);

    public ValueTask CloseMenuAsync(TagId callerId) =>
        EnsureProviderInitialized().CloseMenuAsync(callerId);

    public ValueTask CloseAllMenusAsync() =>
        EnsureProviderInitialized().CloseAllMenusAsync();
}
