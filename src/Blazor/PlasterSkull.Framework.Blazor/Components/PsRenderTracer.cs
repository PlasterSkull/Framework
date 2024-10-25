using Cysharp.Text;
using System.Diagnostics;

namespace PlasterSkull.Framework.Blazor;

public class PsRenderTracer : IDisposable
{
    private static volatile int _globalRenderTracerCounter;

    private readonly int _renderTracerId;
    private PsRenderTracerOptions _renderTracerOptions;
    private Tracer _tracer;
    private int _renderCalls = 1; // Count as initial call on ctor
    private int _renderCount = 0;
    private int _onParametersSetCallCount = 1; // Count as initial call on ctor
    private Stopwatch _renderTimer = new();

    public TagId TagId { get; }

    public CssBuilder ClassNameBuilder =>
        new CssBuilder()
            .AddClass("ps-show-render-info");

    public StyleBuilder StyleNameBuilder =>
        new StyleBuilder()
            .AddStyle("--render-info-font-size", _renderTracerOptions!.FontSize)
            .AddStyle(
                "--render-info-margin",
                _renderTracerOptions!.Margin,
                !string.IsNullOrEmpty(_renderTracerOptions!.Margin))
            .AddStyle("--render-info-color", _renderTracerOptions!.RenderInfoHexColor)
            .AddStyle("--render-info-left-offset", _renderTracerOptions!.Origin.AbsoluteLeftOffset())
            .AddStyle("--render-info-top-offset", _renderTracerOptions!.Origin.AbsoluteTopOffset())
            .AddStyle("--render-info-right-offset", _renderTracerOptions!.Origin.AbsoluteRightOffset())
            .AddStyle("--render-info-bottom-offset", _renderTracerOptions!.Origin.AbsoluteBottomOffset())
            .AddStyle("--render-info-z-index", _renderTracerOptions!.ZIndex.ToString());

    public PsRenderTracer(
        TagId tagId,
        PsRenderTracerOptions? renderTracerOptions)
    {
        TagId = tagId;

        _renderTracerId = Interlocked.Increment(ref _globalRenderTracerCounter);
        _renderTracerOptions = PsRenderTracerOptions.CheckValues(renderTracerOptions) with
        {
            Title = renderTracerOptions?.Title ?? TagId.Tag,
        };
        _tracer = new Tracer(
            ZString.Concat(_renderTracerOptions.Title, ":", _renderTracerId),
            static x => Console.WriteLine("@ " + x.Format()));
    }

    public void OnRenderStart()
    {
        _renderTimer!.Restart();
        _renderCalls++;
    }

    public void OnParametersSetCalled()
    {
        if (_renderCalls == 1)
        {
            return;
        }

        _onParametersSetCallCount++;
    }

    public void OnRendered()
    {
        _renderTimer!.Stop();
        _renderCount++;
        _tracer.Point(ZString.Concat(
            "Render №,",
            _renderCount,
            " took ",
            _renderTimer.Elapsed));
    }

    public string GetStateMessage() =>
        ZString.Concat(
            _renderTracerOptions.Title,
            ":",
            _renderTracerId,
            " | rc: ",
            _renderCalls,
            " | psc: ",
            _onParametersSetCallCount);

    public void Dispose()
    {
        _renderTracerOptions = null!;
        _renderTimer.Stop();
        _renderTimer = null!;
        _tracer = null!;
        GC.SuppressFinalize(this);
    }
}
