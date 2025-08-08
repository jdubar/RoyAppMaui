namespace RoyAppMaui.Utilities;
public static class SleepUtilities
{
    public static decimal GetSleepDuration(decimal bedtime, decimal waketime)
    {
        var duration = waketime - bedtime;
        return duration > 0
            ? duration
            : 24 + duration;
    }
}
