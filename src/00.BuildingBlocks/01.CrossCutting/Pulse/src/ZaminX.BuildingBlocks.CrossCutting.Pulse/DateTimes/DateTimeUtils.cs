namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.DateTimes;

/// <summary>
/// متدهای کمکی جهت کار با تاریخ میلادی
/// </summary>
public static class DateTimeUtils
{
    /// <summary>
    /// Iran Standard Time / Asia-Tehran
    /// </summary>
    public static readonly TimeZoneInfo IranStandardTime = ResolveIranTimeZone();

    /// <summary>
    /// Epoch represented as DateTime
    /// </summary>
    public static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private static TimeZoneInfo ResolveIranTimeZone()
    {
        var knownIds = new[]
        {
            "Asia/Tehran",
            "Iran Standard Time"
        };

        foreach (var id in knownIds)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(id);
            }
            catch
            {
                // ignore and continue
            }
        }

        var matchedTimeZone = TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(timeZoneInfo =>
            timeZoneInfo.StandardName.Contains("Iran", StringComparison.OrdinalIgnoreCase) ||
            timeZoneInfo.StandardName.Contains("Tehran", StringComparison.OrdinalIgnoreCase) ||
            timeZoneInfo.Id.Contains("Iran", StringComparison.OrdinalIgnoreCase) ||
            timeZoneInfo.Id.Contains("Tehran", StringComparison.OrdinalIgnoreCase));

        return matchedTimeZone ?? throw new PlatformNotSupportedException(
            $"This OS [{System.Runtime.InteropServices.RuntimeInformation.OSDescription}] doesn't support IranStandardTime.");
    }

    public static int GetAge(this DateTimeOffset birthday, DateTime comparisonBase, DateTimeOffsetPart dateTimeOffsetPart = DateTimeOffsetPart.IranLocalDateTime)
    {
        return birthday.GetDateTimeOffsetPart(dateTimeOffsetPart).GetAge(comparisonBase);
    }

    public static int GetAge(this DateTimeOffset birthday)
    {
        return birthday.UtcDateTime.GetAge(DateTime.UtcNow);
    }

    public static int GetAge(this DateTime birthday, DateTime comparisonBase)
    {
        var age = comparisonBase.Year - birthday.Year;
        if (comparisonBase < birthday.AddYears(age))
        {
            age--;
        }

        return age;
    }

    public static int GetAge(this DateTime birthday)
    {
        var now = birthday.Kind.GetNow();
        return birthday.GetAge(now);
    }

    public static DateTime GetDateTimeOffsetPart(this DateTimeOffset dateTimeOffset, DateTimeOffsetPart dataDateTimeOffsetPart)
    {
        return dataDateTimeOffsetPart switch
        {
            DateTimeOffsetPart.DateTime => dateTimeOffset.DateTime,
            DateTimeOffsetPart.LocalDateTime => dateTimeOffset.LocalDateTime,
            DateTimeOffsetPart.UtcDateTime => dateTimeOffset.UtcDateTime,
            DateTimeOffsetPart.IranLocalDateTime => dateTimeOffset.ToIranTimeZoneDateTimeOffset().DateTime,
            _ => throw new ArgumentOutOfRangeException(nameof(dataDateTimeOffsetPart), dataDateTimeOffsetPart, null)
        };
    }

    public static DateTime GetNow(this DateTimeKind dataDateTimeKind)
    {
        return dataDateTimeKind == DateTimeKind.Utc ? DateTime.UtcNow : DateTime.Now;
    }

    public static DateTimeOffset ToIranTimeZoneDateTimeOffset(this DateTimeOffset dateTimeOffset)
    {
        return TimeZoneInfo.ConvertTime(dateTimeOffset, IranStandardTime);
    }

    public static DateTime ToIranTimeZoneDateTime(this DateTime dateTime)
    {
        return dateTime.Kind switch
        {
            DateTimeKind.Utc => TimeZoneInfo.ConvertTimeFromUtc(dateTime, IranStandardTime),
            DateTimeKind.Local => TimeZoneInfo.ConvertTime(dateTime, IranStandardTime),
            _ => dateTime
        };
    }

    public static long ToEpochMilliseconds(this DateTime dateTime)
    {
        return (long)dateTime.ToUniversalTime().Subtract(Epoch).TotalMilliseconds;
    }

    public static long ToEpochSeconds(this DateTime dateTime)
    {
        return dateTime.ToEpochMilliseconds() / 1000;
    }

    public static bool IsBetween(this DateTime date, DateTime startDate, DateTime endDate, bool compareTime = false)
    {
        return compareTime
            ? date >= startDate && date <= endDate
            : date.Date >= startDate.Date && date.Date <= endDate.Date;
    }

    public static bool IsLastDayOfTheMonth(this DateTime dateTime)
    {
        return dateTime.Date == new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1).Date;
    }

    public static bool IsWeekend(this DateTime value)
    {
        return value.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }

    public static bool IsLeapYear(this DateTime value)
    {
        return DateTime.DaysInMonth(value.Year, 2) == 29;
    }

    public static DateTimeOffset ToDateTimeOffset(this DateTime dt, TimeSpan offset)
    {
        return dt == DateTime.MinValue ? DateTimeOffset.MinValue : new DateTimeOffset(dt.Ticks, offset);
    }

    public static DateTimeOffset ToDateTimeOffset(this DateTime dt, double offsetInHours = 0)
    {
        return dt.ToDateTimeOffset(offsetInHours == 0 ? TimeSpan.Zero : TimeSpan.FromHours(offsetInHours));
    }
}
