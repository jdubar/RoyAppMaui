using RoyAppMaui.Extensions;

namespace RoyAppMaui.Models;
public class Sleep
{
    public string Id { get; set; } = string.Empty;

    public TimeSpan? Bedtime { get; set; } = new TimeSpan(0, 0, 0);
    public decimal BedtimeRec => Bedtime.ToHoursAsDecimal();
    public string BedtimeDisplay => Bedtime.ToTimeAsString();

    public TimeSpan? Waketime { get; set; } = new TimeSpan(0, 0, 0);
    public decimal WaketimeRec => Waketime.ToHoursAsDecimal();
    public string WaketimeDisplay => Waketime.ToTimeAsString();

    public decimal Duration { get; set; }
}
