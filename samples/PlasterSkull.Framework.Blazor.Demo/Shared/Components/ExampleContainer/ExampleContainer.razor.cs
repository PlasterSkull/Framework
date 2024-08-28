namespace PlasterSkull.Framework.Blazor.Demo.Shared;

public partial class ExampleContainer : PsComponentBase
{
    #region Params

    [Parameter] public RenderFragment? ExampleInfoContent { get; set; }  
    [Parameter] public RenderFragment? DemoContent { get; set; }  
    [Parameter] public RenderFragment? CodeContent { get; set; }  

    #endregion
}
