namespace PlasterSkull.Framework.Blazor.Fluxor;

public record ActionWrapper<T> where T : IPsAction
{
    public required T Action { get; init; }

    public TagId? TagId { get; init; }
    public CancellationToken CancellationToken { get; init; } = CancellationToken.None;
}
