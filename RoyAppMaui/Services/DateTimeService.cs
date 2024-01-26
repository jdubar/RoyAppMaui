using RoyAppMaui.Interfaces;

using System.Globalization;

namespace RoyAppMaui.Services;
public class DateTimeService : IDateTimeService
{
    public decimal GetDuration(decimal bedtime, decimal waketime)
    {
        var duration = waketime - bedtime;
        return duration > 0
                        ? duration
                        : 24 + duration;
    }

    public TimeSpan StringToTimeSpan(string time)
    {
        string[] formats = ["hhmm", "hmm", @"hh\:mm", @"h\:mm\:ss", @"h:mm", @"h:mm tt"];
        var dateTime = DateTime.ParseExact(time, formats, CultureInfo.InvariantCulture);
        return dateTime.TimeOfDay;
    }

    public string TimeSpanToDateTime(TimeSpan newTime) =>
        DateTime.Today.Add(newTime).ToString("hh:mm tt");

    public decimal TimeSpanToDecimal(TimeSpan? newTime) =>
        decimal.Round(Convert.ToDecimal(TimeSpan.Parse(newTime.ToString() ?? "0:0", CultureInfo.InvariantCulture).TotalHours), 2);
}
