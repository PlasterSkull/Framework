using PlasterSkull.Framework.Blazor.Demo.Pages.Components.ContextMenu.Components;
using PlasterSkull.Framework.Blazor.Demo.Shared;
using System.Collections.Frozen;

namespace PlasterSkull.Framework.Blazor.Demo.Pages.Components.ContextMenu;

[Route(PsNavigationManager.ComponentsRoutes.ContextMenu)]
public partial class ContextMenuPage : PsComponentBase
{
    #region Fields

    private static readonly FrozenSet<CodeExampleKey> _basicUsageExampleKeys = new List<CodeExampleKey>
    {
        new(nameof(BasicContextMenuExample)),
    }.ToFrozenSet();

    #endregion
}
