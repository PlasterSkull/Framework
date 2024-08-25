using System.Diagnostics;

namespace PlasterSkull.Framework.Blazor;

public class PsRenderTracer : IDisposable
{
    private PsRenderTracerOptions _renderTracerOptions;
    private int _renderCalls = 0;
    private int _renderCount = 0;
    private Stopwatch _renderTimer = new();
    private int _onParametersSetCallCount;

    public TagId TagId { get; }
    public CssBuilder ClassNameBuilder { get; }
    public StyleBuilder StyleNameBuilder { get; }

    public PsRenderTracer(
        TagId tagId,
        PsRenderTracerOptions? renderTracerOptions)
    {
        TagId = tagId;

        _renderTracerOptions = PsRenderTracerOptions.CheckValues(renderTracerOptions);
        ClassNameBuilder = new CssBuilder()
            .AddClass("ps-show-render-info");
        StyleNameBuilder = new StyleBuilder()
            .AddStyle("--render-info-font-size", _renderTracerOptions!.RenderInfoFontSize)
            .AddStyle(
                "--render-info-margin",
                _renderTracerOptions!.RenderInfoMargin,
                !string.IsNullOrEmpty(_renderTracerOptions!.RenderInfoMargin))
            .AddStyle("--render-info-color", _renderTracerOptions!.RenderInfoHexColor)
            .AddStyle("--render-info-left-offset", _renderTracerOptions!.RenderInfoOrigin.AbsoluteLeftOffset())
            .AddStyle("--render-info-top-offset", _renderTracerOptions!.RenderInfoOrigin.AbsoluteTopOffset())
            .AddStyle("--render-info-right-offset", _renderTracerOptions!.RenderInfoOrigin.AbsoluteRightOffset())
            .AddStyle("--render-info-bottom-offset", _renderTracerOptions!.RenderInfoOrigin.AbsoluteBottomOffset())
            .AddStyle("--render-info-z-index", _renderTracerOptions!.ZIndex.ToString());
    }

    public void OnRenderStart()
    {
        _renderTimer!.Restart();
        _renderCalls++;
    }

    public void OnParametersSetCalled() =>
        _onParametersSetCallCount++;

    public void OnRendered()
    {
        _renderTimer!.Stop();
        _renderCount++;
        Tracer.Default.Point($"#{_renderCount} {TagId.Value} took {_renderTimer.Elapsed}");
    }

    public string GetStateMessage() =>
        $"{TagId.Value} | rc: {_renderCalls} | psc: {_onParametersSetCallCount}";

    public void Dispose()
    {
        _renderTracerOptions = null!;
        _renderTimer.Stop();
        _renderTimer = null!;
        GC.SuppressFinalize(this);
    }
}
