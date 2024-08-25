using ActualLab;
using System.Text.RegularExpressions;

namespace PlasterSkull.Framework.Core;

public static partial class StringExt
{
    [GeneratedRegex("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])", RegexOptions.Compiled)]
    private static partial Regex KebabCaseRegex();

    public static string ToKebabCase(this string value) =>
        value.IsNullOrEmpty()
            ? value
            : KebabCaseRegex()
                .Replace(value, "-$1")
                .Trim()
                .ToLower();

    [GeneratedRegex(@"\s+")]
    private static partial Regex RemoveMultipleSpaces();

    public static string RemoveMultipleSpaces(string input) =>
        RemoveMultipleSpaces().Replace(input, " ").Trim();
}
