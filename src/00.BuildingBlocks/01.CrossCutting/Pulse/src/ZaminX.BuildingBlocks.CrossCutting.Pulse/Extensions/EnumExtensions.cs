using System.ComponentModel;

namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// برای دریافت توضیحات یک ویژگی از enum اگر [Description] داشته باشد از این متد استفاده می‌شود.
    /// </summary>
    public static string GetEnumDescription(this Enum enumValue)
    {
        ArgumentNullException.ThrowIfNull(enumValue);

        var memberInfo = enumValue.GetType().GetField(enumValue.ToString());
        var attribute = memberInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .OfType<DescriptionAttribute>()
            .FirstOrDefault();

        return string.IsNullOrWhiteSpace(attribute?.Description)
            ? enumValue.ToString()
            : attribute.Description;
    }
}
