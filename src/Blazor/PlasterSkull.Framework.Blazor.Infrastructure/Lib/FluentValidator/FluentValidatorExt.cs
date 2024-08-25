using FluentValidation;

namespace Etai.Blazor;

public static class FluentValidatorExt
{
    public static IRuleBuilderOptions<T, string> DisableSpecialChars<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder.Matches("^(?=.*[A-Za-z0-9]$)[A-Za-z][A-Za-z\\d.-]{0,63}$");
}
