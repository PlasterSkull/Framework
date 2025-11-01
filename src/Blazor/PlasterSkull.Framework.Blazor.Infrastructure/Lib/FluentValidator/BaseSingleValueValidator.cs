using FluentValidation;
using System.Data;

namespace PlasterSkull.Framework.Blazor;

public class BaseSingleValueValidator<T> : AbstractValidator<T>
{
    public BaseSingleValueValidator(Action<IRuleBuilderInitial<T, T>>? rule = null)
    {
        rule?.Invoke(RuleFor(x => x));
    }

    public Func<T, IEnumerable<string>> Validation => ValidateValue;

    private IEnumerable<string> ValidateValue(T arg)
    {
        var result = Validate(arg);
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    }
}
