using PlasterSkull.Framework.Blazor.Demo.Pages.Features.RenderTracer.Components;
using PlasterSkull.Framework.Blazor.Demo.Pages.Features.RenderTracer.Components.NestingLevelRenderTracerExample;
using PlasterSkull.Framework.Blazor.Demo.Pages.Features.RenderTracer.Components.NestingLevelRenderTracerExample.Components;
using PlasterSkull.Framework.Blazor.Demo.Shared;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace PlasterSkull.Framework.Blazor.Demo.Pages.Features.RenderTracer;

[Route(PsNavigationManager.FeaturesRoutes.RenderTracer)]
public partial class RenderTracerPage
{
    #region Fields

    private static readonly FrozenSet<CodeExampleKey> _basicUsageExampleKeys = new List<CodeExampleKey>
    {
        new(nameof(BasicRenderTracerExample)),
    }.ToFrozenSet();

    private static readonly FrozenSet<CodeExampleKey> _simulatingRealTimeEventsExampleKeys = new List<CodeExampleKey>
    {
        new(nameof(RealTimeEventsRenderTracerExample)),
    }.ToFrozenSet();

    private static readonly ImmutableList<CodeExampleKey> _nestingLevelEventsExampleKeys =
    [
        new(nameof(NestingLevelRenderTracerExample)),
        new(nameof(NestingLevelOneBlockExample)),
        new(nameof(NestingLevelTwoBlockExample)),
        new(nameof(NestingLevelThreeBlockExample)),
    ];

    #endregion
}
