namespace PlasterSkull.Framework.Blazor;

public abstract class PsContextMenuTriggerComponentBase : PsComponentBase
{
    #region Params

    [Parameter] public string? Title { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool ClickPropagation { get; set; } = true;
    [Parameter] public bool IgnoreLongTap { get; set; }
    [Parameter] public bool FullScreenMobile { get; set; }
    [Parameter] public bool HideMobileHeader { get; set; }
    [Parameter] public PsContextMenuActivateBehavior ActivationEvent { get; set; }
    [Parameter] public Origin Origin { get; set; } = Origin.BottomRight;

    [Parameter] public string? ShownClass { get; set; }
    [Parameter] public string? HiddenClass { get; set; }

    [Parameter] public EventCallback OnHiding { get; set; }
    [Parameter] public EventCallback OnAppearing { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? MenuContent { get; set; }

    #endregion
}
