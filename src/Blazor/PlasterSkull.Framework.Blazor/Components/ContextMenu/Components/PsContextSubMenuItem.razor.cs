namespace PlasterSkull.Framework.Blazor;

public partial class PsContextSubMenuItem : PsContextMenuTriggerComponentBase
{
    [Parameter] public string? Text { get; set; }
    [Parameter] public string? Icon { get; set; }
}
