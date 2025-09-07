namespace RoyAppMaui.Extensions;

/// <summary>
/// Extension methods for <see cref="TimeSpan"/>.
/// </summary>
public static class TimeSpanExtensions
{
    /// <summary>
    /// Converts a <see cref="TimeSpan"/> to a 12-hour time string (e.g., "08:30 PM").
    /// </summary>
    /// <param name="time">The time span to convert.</param>
    /// <returns>A string representation in "hh:mm tt" format.</returns>
    public static string ToTimeAsString(this TimeSpan time)
    {
        if (time < TimeSpan.Zero)
        {
            return "Invalid Time";
        }

        // Normalize to 24-hour clock for very large values
        var normalized = TimeSpan.FromTicks(time.Ticks % TimeSpan.FromDays(1).Ticks);
        var dateTime = DateTime.Today.Add(normalized);
        return dateTime.ToString("hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Converts a <see cref="TimeSpan"/> to a decimal representing total hours, rounded to 2 decimals.
    /// </summary>
    /// <param name="time">The time span to convert.</param>
    /// <returns>Total hours as a decimal, rounded to 2 decimals.</returns>
    public static decimal ToHoursAsDecimal(this TimeSpan time)
    {
        var totalHoursDecimal = (decimal)time.TotalHours;
        return decimal.Round(totalHoursDecimal, 2);
    }
}
