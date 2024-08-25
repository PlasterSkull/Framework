namespace PlasterSkull.Framework.Blazor;

public partial class PsRender : ComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
