using System.Globalization;

namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.DateTimes;

/// <summary>
/// Represents PersianDateTime utils.
/// </summary>
public static class PersianDateTimeUtils
{
    public static bool IsValidPersianDate(int persianYear, int persianMonth, int persianDay)
    {
        if (persianDay is <= 0 or > 31)
        {
            return false;
        }

        if (persianMonth is <= 0 or > 12)
        {
            return false;
        }

        if (persianMonth <= 6 && persianDay > 31)
        {
            return false;
        }

        if (persianMonth is >= 7 and <= 11 && persianDay > 30)
        {
            return false;
        }

        if (persianMonth == 12)
        {
            var persianCalendar = new PersianCalendar();
            var isLeapYear = persianCalendar.IsLeapYear(persianYear);

            if (isLeapYear && persianDay > 30)
            {
                return false;
            }

            if (!isLeapYear && persianDay > 29)
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsValidPersianDateTime(this string? persianDateTime)
    {
        if (string.IsNullOrWhiteSpace(persianDateTime))
        {
            return false;
        }

        try
        {
            return persianDateTime.ToGregorianDateTime().HasValue;
        }
        catch
        {
            return false;
        }
    }

    public static DateTime? ToGregorianDateTime(this string? persianDateTime)
    {
        if (string.IsNullOrWhiteSpace(persianDateTime))
        {
            return null;
        }

        persianDateTime = persianDateTime.Trim().ToEnglishNumbers();
        var splitedDateTime = persianDateTime.Split([' '], StringSplitOptions.RemoveEmptyEntries);

        var rawTime = splitedDateTime.FirstOrDefault(static s => s.Contains(':'));
        var rawDate = splitedDateTime.FirstOrDefault(static s => !s.Contains(':'));

        var splitedDate = rawDate?.Split('/', ',', '؍', '.', '-', '\\');
        if (splitedDate?.Length != 3)
        {
            return null;
        }

        var day = GetDay(splitedDate[2]);
        var month = GetMonth(splitedDate[1]);
        var year = GetYear(splitedDate[0]);

        if (!day.HasValue || !month.HasValue || !year.HasValue)
        {
            return null;
        }

        if (!IsValidPersianDate(year.Value, month.Value, day.Value))
        {
            return null;
        }

        var hour = 0;
        var minute = 0;
        var second = 0;

        if (!string.IsNullOrWhiteSpace(rawTime))
        {
            var splitedTime = rawTime.Split([':'], StringSplitOptions.RemoveEmptyEntries);
            if (splitedTime.Length < 2)
            {
                return null;
            }

            if (!int.TryParse(splitedTime[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out hour) ||
                !int.TryParse(splitedTime[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out minute))
            {
                return null;
            }

            if (splitedTime.Length > 2)
            {
                var lastPart = splitedTime[2].Trim();
                var formatInfo = PersianCulture.Instance.DateTimeFormat;

                if (lastPart.Equals(formatInfo.PMDesignator, StringComparison.OrdinalIgnoreCase))
                {
                    if (hour < 12)
                    {
                        hour += 12;
                    }
                }
                else if (!int.TryParse(lastPart, NumberStyles.Integer, CultureInfo.InvariantCulture, out second))
                {
                    return null;
                }
            }
        }

        var persianCalendar = new PersianCalendar();
        return persianCalendar.ToDateTime(year.Value, month.Value, day.Value, hour, minute, second, 0);
    }

    public static DateTimeOffset? ToGregorianDateTimeOffset(this string? persianDateTime)
    {
        var dateTime = persianDateTime.ToGregorianDateTime();
        return dateTime is null ? null : new DateTimeOffset(dateTime.Value, DateTimeUtils.IranStandardTime.BaseUtcOffset);
    }

    public static string ToLongPersianDateString(this DateTime dt)
    {
        return dt.ToPersianDateTimeString(PersianCulture.Instance.DateTimeFormat.LongDatePattern);
    }

    public static string ToLongPersianDateString(this DateTime? dt)
    {
        return dt is null ? string.Empty : dt.Value.ToLongPersianDateString();
    }

    public static string ToLongPersianDateString(this DateTimeOffset? dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        return dt is null ? string.Empty : dt.Value.GetDateTimeOffsetPart(dateTimeOffsetPart).ToLongPersianDateString();
    }

    public static string ToLongPersianDateString(this DateTimeOffset dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        return dt.GetDateTimeOffsetPart(dateTimeOffsetPart).ToLongPersianDateString();
    }

    public static string ToLongPersianDateTimeString(this DateTime dt)
    {
        return dt.ToPersianDateTimeString($"{PersianCulture.Instance.DateTimeFormat.LongDatePattern}، {PersianCulture.Instance.DateTimeFormat.LongTimePattern}");
    }

    public static string ToLongPersianDateTimeString(this DateTime? dt)
    {
        return dt is null ? string.Empty : dt.Value.ToLongPersianDateTimeString();
    }

    public static string ToLongPersianDateTimeString(this DateTimeOffset? dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        return dt is null ? string.Empty : dt.Value.GetDateTimeOffsetPart(dateTimeOffsetPart).ToLongPersianDateTimeString();
    }

    public static string ToLongPersianDateTimeString(this DateTimeOffset dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        return dt.GetDateTimeOffsetPart(dateTimeOffsetPart).ToLongPersianDateTimeString();
    }

    public static string ToPersianDateTimeString(this DateTime dateTime, string format)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(format);
        return dateTime.ToString(format, PersianCulture.Instance);
    }

    public static PersianDay ToPersianYearMonthDay(this DateTimeOffset? gregorianDate, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        return gregorianDate is null
            ? throw new ArgumentNullException(nameof(gregorianDate))
            : gregorianDate.Value.GetDateTimeOffsetPart(dateTimeOffsetPart).ToPersianYearMonthDay();
    }

    public static PersianDay ToPersianYearMonthDay(this DateTime? gregorianDate)
    {
        return gregorianDate is null ? throw new ArgumentNullException(nameof(gregorianDate)) : gregorianDate.Value.ToPersianYearMonthDay();
    }

    public static PersianDay ToPersianYearMonthDay(this DateTimeOffset gregorianDate, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        return gregorianDate.GetDateTimeOffsetPart(dateTimeOffsetPart).ToPersianYearMonthDay();
    }

    public static PersianDay ToPersianYearMonthDay(this DateTime gregorianDate)
    {
        var persianCalendar = new PersianCalendar();
        return new PersianDay(
            persianCalendar.GetYear(gregorianDate),
            persianCalendar.GetMonth(gregorianDate),
            persianCalendar.GetDayOfMonth(gregorianDate));
    }

    public static string ToShortPersianDateString(this DateTimeOffset? dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        return dt is null ? string.Empty : dt.Value.GetDateTimeOffsetPart(dateTimeOffsetPart).ToShortPersianDateString();
    }

    public static string ToShortPersianDateString(this DateTimeOffset dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        return dt.GetDateTimeOffsetPart(dateTimeOffsetPart).ToShortPersianDateString();
    }

    public static string ToShortPersianDateString(this DateTime dt)
    {
        return dt.ToPersianDateTimeString(PersianCulture.Instance.DateTimeFormat.ShortDatePattern);
    }

    public static string ToShortPersianDateString(this DateTime? dt)
    {
        return dt is null ? string.Empty : dt.Value.ToShortPersianDateString();
    }

    public static string ToShortPersianDateTimeString(this DateTime dt)
    {
        return dt.ToPersianDateTimeString($"{PersianCulture.Instance.DateTimeFormat.ShortDatePattern} {PersianCulture.Instance.DateTimeFormat.ShortTimePattern}");
    }

    public static string ToShortPersianDateTimeString(this DateTime? dt)
    {
        return dt is null ? string.Empty : dt.Value.ToShortPersianDateTimeString();
    }

    public static string ToShortPersianDateTimeString(this DateTimeOffset? dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        return dt is null ? string.Empty : dt.Value.GetDateTimeOffsetPart(dateTimeOffsetPart).ToShortPersianDateTimeString();
    }

    public static string ToShortPersianDateTimeString(this DateTimeOffset dt, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        return dt.GetDateTimeOffsetPart(dateTimeOffsetPart).ToShortPersianDateTimeString();
    }

    public static string CorrectReverseDate(string reverseDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reverseDate);

        reverseDate = reverseDate.Trim();
        var dateParts = reverseDate.Split('/', ',', '؍', '.', '-', '\\');
        return dateParts.Length == 3 && dateParts[2].Length == 4
            ? $"{dateParts[2]}/{dateParts[1]}/{dateParts[0]}"
            : reverseDate;
    }

    private static int? GetDay(string part)
    {
        var day = part.ToEnglishInt();
        return day is > 0 and <= 31 ? day : null;
    }

    private static int? GetMonth(string part)
    {
        var month = part.ToEnglishInt();
        return month is > 0 and <= 12 ? month : null;
    }

    private static int? GetYear(string part)
    {
        var year = part.ToEnglishInt();
        if (!year.HasValue)
        {
            return null;
        }

        return part.Length == 2 ? year + 1300 : year;
    }

    private static int? ToEnglishInt(this string? data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            return null;
        }

        return int.TryParse(data.ToEnglishNumbers(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var number)
            ? number
            : null;
    }
}
