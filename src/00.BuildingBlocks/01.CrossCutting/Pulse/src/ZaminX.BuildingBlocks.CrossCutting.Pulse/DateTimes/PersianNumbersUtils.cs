using System.Globalization;

namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.DateTimes;

/// <summary>
/// Converts English digits of a given number to their equivalent Persian digits.
/// </summary>
public static class PersianNumbersUtils
{
    public static string ToPersianNumbers(this int number, string format = "")
    {
        return (!string.IsNullOrEmpty(format) ? number.ToString(format, CultureInfo.InvariantCulture) : number.ToString(CultureInfo.InvariantCulture))
            .ToPersianNumbers();
    }

    public static string ToPersianNumbers(this long number, string format = "")
    {
        return (!string.IsNullOrEmpty(format) ? number.ToString(format, CultureInfo.InvariantCulture) : number.ToString(CultureInfo.InvariantCulture))
            .ToPersianNumbers();
    }

    public static string ToPersianNumbers(this int? number, string format = "")
    {
        var value = number ?? 0;
        return (!string.IsNullOrEmpty(format) ? value.ToString(format, CultureInfo.InvariantCulture) : value.ToString(CultureInfo.InvariantCulture))
            .ToPersianNumbers();
    }

    public static string ToPersianNumbers(this long? number, string format = "")
    {
        var value = number ?? 0;
        return (!string.IsNullOrEmpty(format) ? value.ToString(format, CultureInfo.InvariantCulture) : value.ToString(CultureInfo.InvariantCulture))
            .ToPersianNumbers();
    }

    public static string ToPersianNumbers(this string? data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            return string.Empty;
        }

        return data.ToEnglishNumbers()
            .Replace("0", "۰")
            .Replace("1", "۱")
            .Replace("2", "۲")
            .Replace("3", "۳")
            .Replace("4", "۴")
            .Replace("5", "۵")
            .Replace("6", "۶")
            .Replace("7", "۷")
            .Replace("8", "۸")
            .Replace("9", "۹")
            .Replace(".", ",");
    }

    public static string ToEnglishNumbers(this string? data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            return string.Empty;
        }

        return data.Replace("٠", "0")
            .Replace("۰", "0")
            .Replace("١", "1")
            .Replace("۱", "1")
            .Replace("٢", "2")
            .Replace("۲", "2")
            .Replace("٣", "3")
            .Replace("۳", "3")
            .Replace("٤", "4")
            .Replace("۴", "4")
            .Replace("٥", "5")
            .Replace("۵", "5")
            .Replace("٦", "6")
            .Replace("۶", "6")
            .Replace("٧", "7")
            .Replace("۷", "7")
            .Replace("٨", "8")
            .Replace("۸", "8")
            .Replace("٩", "9")
            .Replace("۹", "9");
    }
}
