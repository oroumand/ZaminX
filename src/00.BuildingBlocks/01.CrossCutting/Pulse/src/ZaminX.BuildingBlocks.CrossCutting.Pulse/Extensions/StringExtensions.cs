using System.Globalization;
using System.Text;

namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Extensions;

public static class StringExtensions
{
    public const char ArabicYeChar = (char)1610;
    public const char PersianYeChar = (char)1740;

    public const char ArabicKeChar = (char)1603;
    public const char PersianKeChar = (char)1705;

    public static string? ApplyCorrectYeKe(this object? data)
    {
        return data is null ? null : ApplyCorrectYeKe(data.ToString());
    }

    public static string ApplyCorrectYeKe(this string? data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            return string.Empty;
        }

        return data.Replace(ArabicYeChar, PersianYeChar)
            .Replace(ArabicKeChar, PersianKeChar)
            .Trim()
            .Fa2En();
    }

    public static long ToSafeLong(this string? input, long replacement = long.MinValue) =>
        long.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) ? result : replacement;

    public static long? ToSafeNullableLong(this string? input) =>
        long.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) ? result : null;

    public static int ToSafeInt(this string? input, int replacement = int.MinValue) =>
        int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) ? result : replacement;

    public static int? ToSafeNullableInt(this string? input) =>
        int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) ? result : null;

    public static string ToStringOrEmpty(this string? input) => input ?? string.Empty;

    public static string ToUnderscoreCase(this string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return string.Concat(input.Select((character, index) =>
            index > 0 && char.IsUpper(character) ? "_" + character : character.ToString())).ToLowerInvariant();
    }

    public static byte[] ToByteArray(this string input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return Encoding.UTF8.GetBytes(input);
    }

    public static string FromByteArray(this byte[] input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return Encoding.UTF8.GetString(input);
    }

    public static string ToNumeric(this int value) => value.ToString("N0", CultureInfo.InvariantCulture);

    public static string ToCurrency(this int value) => value.ToString("C0", CultureInfo.CurrentCulture);

    public static string En2Fa(this string str)
    {
        ArgumentNullException.ThrowIfNull(str);

        return str.Replace("0", "۰")
            .Replace("1", "۱")
            .Replace("2", "۲")
            .Replace("3", "۳")
            .Replace("4", "۴")
            .Replace("5", "۵")
            .Replace("6", "۶")
            .Replace("7", "۷")
            .Replace("8", "۸")
            .Replace("9", "۹");
    }

    public static string Fa2En(this string str)
    {
        ArgumentNullException.ThrowIfNull(str);

        return str.Replace("۰", "0")
            .Replace("۱", "1")
            .Replace("۲", "2")
            .Replace("۳", "3")
            .Replace("۴", "4")
            .Replace("۵", "5")
            .Replace("۶", "6")
            .Replace("۷", "7")
            .Replace("۸", "8")
            .Replace("۹", "9")
            .Replace("٠", "0")
            .Replace("١", "1")
            .Replace("٢", "2")
            .Replace("٣", "3")
            .Replace("٤", "4")
            .Replace("٥", "5")
            .Replace("٦", "6")
            .Replace("٧", "7")
            .Replace("٨", "8")
            .Replace("٩", "9");
    }
}
