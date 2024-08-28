namespace PlasterSkull.Framework.Blazor;

public partial class PsBreakpointResolver : ComponentBase
{
    #region Params

    [Parameter] public RenderFragment? Desktop { get; set; }
    [Parameter] public RenderFragment? Mobile { get; set; }

    #endregion
}
