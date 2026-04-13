using System.Text.RegularExpressions;
using ZaminX.BuildingBlocks.CrossCutting.Pulse.DateTimes;

namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Extensions;

public static partial class StringValidatorExtensions
{
    /// <summary>
    /// صحت سنجی کد ملی
    /// </summary>
    public static bool IsNationalCode(this string? nationalCode)
    {
        if (string.IsNullOrWhiteSpace(nationalCode))
        {
            return false;
        }

        nationalCode = nationalCode.ToEnglishNumbers().Trim();

        if (!nationalCode.IsLengthBetween(8, 10))
        {
            return false;
        }

        nationalCode = nationalCode.PadLeft(10, '0');

        if (!nationalCode.IsNumeric())
        {
            return false;
        }

        if (AllSame10Digits().Contains(nationalCode))
        {
            return false;
        }

        var digits = nationalCode.Select(c => c - '0').ToArray();
        var checksum = digits[9];
        var sum = 0;
        for (var i = 0; i < 9; i++)
        {
            sum += digits[i] * (10 - i);
        }

        var remainder = sum % 11;
        return (remainder < 2 && checksum == remainder) || (remainder >= 2 && checksum == 11 - remainder);
    }

    /// <summary>
    /// صحت سنجی شناسه ملی شرکت‌ها
    /// </summary>
    public static bool IsLegalNationalIdValid(this string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
        {
            return false;
        }

        nationalId = nationalId.ToEnglishNumbers().Trim();

        if (!nationalId.IsLengthEqual(11) || !nationalId.IsNumeric())
        {
            return false;
        }

        if (AllSame11Digits().Contains(nationalId))
        {
            return false;
        }

        var digits = nationalId.Select(c => c - '0').ToArray();
        var controlCode = digits[10];
        var factor = digits[9] + 2;

        var sum =
            (factor + digits[0]) * 29 +
            (factor + digits[1]) * 27 +
            (factor + digits[2]) * 23 +
            (factor + digits[3]) * 19 +
            (factor + digits[4]) * 17 +
            (factor + digits[5]) * 29 +
            (factor + digits[6]) * 27 +
            (factor + digits[7]) * 23 +
            (factor + digits[8]) * 19 +
            (factor + digits[9]) * 17;

        var remainder = sum % 11;
        if (remainder == 10)
        {
            remainder = 0;
        }

        return remainder == controlCode;
    }

    public static bool IsNumeric(this string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        input = input.ToEnglishNumbers().Trim();
        return NumericRegex().IsMatch(input);
    }

    public static bool IsLengthBetween(this string? input, int minLength, int maxLenght)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(minLength);
        ArgumentOutOfRangeException.ThrowIfNegative(maxLenght);

        return input is not null && input.Length >= minLength && input.Length <= maxLenght;
    }

    public static bool IsLengthLessThan(this string? input, int lenght) => input is not null && input.Length < lenght;

    public static bool IsLengthLessThanOrEqual(this string? input, int lenght) => input is not null && input.Length <= lenght;

    public static bool IsLengthGreaterThan(this string? input, int lenght) => input is not null && input.Length > lenght;

    public static bool IsLengthGreaterThanOrEqual(this string? input, int lenght) => input is not null && input.Length >= lenght;

    public static bool IsLengthEqual(this string? input, int lenght) => input is not null && input.Length == lenght;

    private static string[] AllSame10Digits() => ["0000000000","1111111111","2222222222","3333333333","4444444444","5555555555","6666666666","7777777777","8888888888","9999999999"];

    private static string[] AllSame11Digits() => ["00000000000","11111111111","22222222222","33333333333","44444444444","55555555555","66666666666","77777777777","88888888888","99999999999"];

    [GeneratedRegex(@"^\d+$", RegexOptions.Compiled)]
    private static partial Regex NumericRegex();
}
