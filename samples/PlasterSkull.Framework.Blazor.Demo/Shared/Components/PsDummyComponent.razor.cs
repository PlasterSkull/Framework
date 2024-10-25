namespace PlasterSkull.Framework.Blazor.Demo.Shared;

public partial class PsDummyComponent : PsComponentBase
{
    #region Params

    [Parameter] public string Text { get; set; } = "Some text";
    [Parameter] public string ButtonText { get; set; } = "Click!";

    [Parameter] public bool EnableRenderTracer { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public EventCallback OnClick { get; set; }

    #endregion

    #region LC Events

    protected override void OnInitialized()
    {
        if (EnableRenderTracer)
        {
            EnableRenderTracing();
        }

        base.OnInitialized();
    }

    #endregion
}
