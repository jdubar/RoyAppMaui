﻿using RoyAppMaui.Models;

namespace RoyAppMaui.Extensions;

/// <summary>
/// Extension methods for calculating averages on Sleep collections.
/// </summary>
public static class SleepExtensions
{
    /// <summary>
    /// Calculates the average of a selected decimal property for a collection of Sleep objects.
    /// Returns 0 if the collection is null or empty.
    /// </summary>
    /// <param name="sleeps">The collection of Sleep objects.</param>
    /// <param name="selector">A function to select the decimal value from each Sleep.</param>
    /// <returns>The average value, rounded to 2 decimals, or 0 if the collection is null or empty.</returns>
    public static decimal GetAverage(this IEnumerable<Sleep> sleeps, Func<Sleep, decimal> selector)
    {
        if (sleeps is null)
        {
            return 0m;
        }

        var sum = 0m;
        var count = 0;
        foreach (var sleep in sleeps)
        {
            sum += selector(sleep);
            count++;
        }

        return count == 0
            ? 0m
            : decimal.Round(sum / count, 2);
    }
}
