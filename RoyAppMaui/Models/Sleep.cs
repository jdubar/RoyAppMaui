﻿using RoyAppMaui.Extensions;

namespace RoyAppMaui.Models;
public class Sleep
{
    public string Id { get; set; } = string.Empty;
    public TimeSpan Bedtime { get; set; } = TimeSpan.Zero;
    public TimeSpan Waketime { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// Bedtime as a decimal hour value (rounded to 2 decimals).
    /// </summary>
    public decimal BedtimeRec => Bedtime.ToHoursAsDecimal();

    /// <summary>
    /// Waketime as a decimal hour value (rounded to 2 decimals).
    /// </summary>
    public decimal WaketimeRec => Waketime.ToHoursAsDecimal();

    /// <summary>
    /// Bedtime as a formatted string (e.g., "10:00 PM").
    /// </summary>
    public string BedtimeDisplay => Bedtime.ToTimeAsString();

    /// <summary>
    /// Waketime as a formatted string (e.g., "06:00 AM").
    /// </summary>
    public string WaketimeDisplay => Waketime.ToTimeAsString();

    /// <summary>
    /// The duration of sleep in hours, calculated using TimeSpan for accuracy.
    /// </summary>
    public decimal Duration => GetSleepDuration(Bedtime, Waketime);

    private static decimal GetSleepDuration(TimeSpan bedtime, TimeSpan waketime)
    {
        // Handles overnight sleep (e.g., 22:00 to 06:00)
        var duration = waketime - bedtime;
        if (duration < TimeSpan.Zero)
        {
            duration += TimeSpan.FromHours(24);
        }

        return duration.TotalHours > 0
            ? Math.Round((decimal)duration.TotalHours, 2)
            : 0m;
    }
}
