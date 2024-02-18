using System.Globalization;

namespace RoyAppMaui.Services.Impl;
public class DateTimeService : IDateTimeService
{
    private readonly CultureInfo _invariant = CultureInfo.InvariantCulture;

    public TimeSpan StringToTimeSpan(string time)
    {
        string[] formats = ["hhmm", "hmm", @"hh\:mm", @"h\:mm\:ss", @"h:mm", @"h:mm tt"];
        var dateTime = DateTime.ParseExact(time, formats, _invariant);
        return dateTime.TimeOfDay;
    }

    public string TimeSpanToDateTime(TimeSpan newTime) =>
        DateTime.Today.Add(newTime).ToString("hh:mm tt");

    public decimal TimeSpanToDecimal(TimeSpan? newTime) =>
        decimal.Round(Convert.ToDecimal(TimeSpan.Parse(newTime.ToString() ?? "0:0", _invariant).TotalHours), 2);
}
