namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.DateTimes;

/// <summary>
/// اجزای روز شمسی
/// </summary>
public sealed class PersianDay : IEquatable<PersianDay>
{
    public int Year { get; set; }

    public int Month { get; set; }

    public int Day { get; set; }

    public PersianDay()
    {
    }

    public PersianDay(int year, int month, int day)
    {
        Year = year;
        Month = month;
        Day = day;
    }

    public override string ToString() => $"{Year}/{Month:00}/{Day:00}";

    public bool Equals(PersianDay? other)
    {
        return other is not null && Year == other.Year && Month == other.Month && Day == other.Day;
    }

    public override bool Equals(object? obj) => Equals(obj as PersianDay);

    public override int GetHashCode() => HashCode.Combine(Year, Month, Day);
}
