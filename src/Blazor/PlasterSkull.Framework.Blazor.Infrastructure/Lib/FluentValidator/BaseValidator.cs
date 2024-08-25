using FluentValidation;

namespace Etai.Blazor;

public abstract class BaseValidator<T> : AbstractValidator<T> where T : class
{
    public Func<object, string, Task<IEnumerable<string>>> Validation => ValidateValues;

    private async Task<IEnumerable<string>> ValidateValues(object model, string propertyName)
    {
        var result = await ValidateAsync(
            ValidationContext<T>.CreateWithOptions(
                (T)model,
                x => x.IncludeProperties(propertyName)));

        return result.IsValid
            ? Array.Empty<string>()
            : result.Errors.Select(e => e.ErrorMessage);
    }
}
