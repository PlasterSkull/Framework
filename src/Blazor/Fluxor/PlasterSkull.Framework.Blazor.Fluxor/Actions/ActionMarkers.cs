using System.Collections.Immutable;

namespace PlasterSkull.Framework.Blazor.Fluxor;

public interface IPsAction { }
public interface IAllowAnonymousAction { }
public interface IHasErrorsAction<TError>
{
    ImmutableList<TError> Errors { get; init; }
}