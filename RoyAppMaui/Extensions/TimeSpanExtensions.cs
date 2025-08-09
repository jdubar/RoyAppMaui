namespace RoyAppMaui.Extensions;
public static class TimeSpanExtensions
{
    public static string ToTimeAsString(this TimeSpan time)
    {
        var dateTime = DateTime.Today.Add(time);
        return dateTime.ToString("hh:mm tt");
    }

    public static decimal ToHoursAsDecimal(this TimeSpan time)
    {
        var totalHoursDecimal = (decimal)time.TotalHours;
        return decimal.Round(totalHoursDecimal, 2);
    }
}
