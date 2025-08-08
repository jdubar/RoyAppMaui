using RoyAppMaui.Models;

namespace RoyAppMaui.Extensions;
public static class SleepExtensions
{
    public static decimal GetAverage(this IEnumerable<Sleep> sleeps, Func<Sleep, decimal> selector)
    {
        if (!sleeps.Any())
        {
            return 0m;
        }

        return decimal.Round(sleeps.ToList().Sum(selector) / sleeps.Count(), 2);
    }
}
