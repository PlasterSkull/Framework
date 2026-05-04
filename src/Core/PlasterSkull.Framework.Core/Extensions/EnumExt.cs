using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.Serialization;

namespace PlasterSkull.Framework.Core;

public static class EnumExt
{
    public static string ExtractNameFromAttributes(this Enum enumValue)
    {
        var enumMemberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
        if (enumMemberInfo == null)
        {
            return string.Empty;
        }

        var dataMemberAttribute = enumMemberInfo.GetCustomAttribute<DataMemberAttribute>();
        if (dataMemberAttribute != null && !string.IsNullOrEmpty(dataMemberAttribute.Name))
        {
            return dataMemberAttribute.Name;
        }

        var displayAttribute = enumMemberInfo.GetCustomAttribute<DisplayAttribute>();
        if (displayAttribute != null && !string.IsNullOrEmpty(displayAttribute.Name))
        {
            return displayAttribute.Name;
        }

        return string.Empty;
    }
}
