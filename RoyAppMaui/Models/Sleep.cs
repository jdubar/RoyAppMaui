using RoyAppMaui.Extensions;

namespace RoyAppMaui.Models;
public class Sleep
{
    public string Id { get; set; } = string.Empty;
    public TimeSpan Bedtime { get; set; } = new TimeSpan(0, 0, 0);
    public TimeSpan Waketime { get; set; } = new TimeSpan(0, 0, 0);

    public decimal BedtimeRec => Bedtime.ToHoursAsDecimal();
    public string BedtimeDisplay => Bedtime.ToTimeAsString();
    public decimal WaketimeRec => Waketime.ToHoursAsDecimal();
    public string WaketimeDisplay => Waketime.ToTimeAsString();
    public decimal Duration => GetSleepDuration(BedtimeRec, WaketimeRec);

    private static decimal GetSleepDuration(decimal bedtime, decimal waketime)
    {
        var duration = waketime - bedtime;
        return duration > 0
            ? duration
            : 24 + duration;
    }
}
