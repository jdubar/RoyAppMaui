namespace RoyAppMaui.Extensions;
public static class TimeSpanExtensions
{
    public static string ToTimeAsString(this TimeSpan? time)
    {
        if (!time.HasValue)
        {
            return "00:00";
        }

        var dateTime = DateTime.Today.Add((TimeSpan)time);
        return dateTime.ToString("hh:mm tt");
    }

    public static decimal ToHoursAsDecimal(this TimeSpan? time)
    {
        if (time is null)
        {
            return 0;
        }

        var totalHoursDecimal = (decimal)time.Value.TotalHours;
        return decimal.Round(totalHoursDecimal, 2);
    }
}
