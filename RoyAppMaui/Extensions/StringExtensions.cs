using System.Globalization;

namespace RoyAppMaui.Extensions;

/// <summary>
/// Extension methods for <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts the string to a UTF-8 byte array.
    /// </summary>
    public static byte[] ToBytes(this string str) => System.Text.Encoding.UTF8.GetBytes(str);

    /// <summary>
    /// Converts a string to a <see cref="TimeSpan"/> using multiple accepted formats.
    /// Throws <see cref="FormatException"/> if parsing fails.
    /// </summary>
    /// <param name="time">The time string to parse.</param>
    /// <returns>The parsed <see cref="TimeSpan"/>.</returns>
    /// <exception cref="FormatException">Thrown if the string cannot be parsed as a time.</exception>
    public static TimeSpan ToTimeSpan(this string time)
    {
        if (string.IsNullOrWhiteSpace(time))
        {
            throw new FormatException("Input string is null or empty.");
        }

        string[] formats = ["hhmm", "hmm", @"hh\:mm", @"h\:mm\:ss", @"h:mm", @"h:mm tt", @"HH\:mm", "HHmm"];
        var dateTime = DateTime.ParseExact(time, formats, CultureInfo.InvariantCulture);
        return dateTime.TimeOfDay;
    }
}
