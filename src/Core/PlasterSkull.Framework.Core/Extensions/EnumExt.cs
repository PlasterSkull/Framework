using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PlasterSkull.Framework.Core;

public static class EnumExt
{
    public static string? ToDisplayString(this Enum enumValue) =>
        enumValue
            .GetType()
            .GetMember(enumValue.ToString())
            .FirstOrDefault()?
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName() ??
        string.Empty;
}
