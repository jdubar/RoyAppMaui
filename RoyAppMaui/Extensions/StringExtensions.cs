using System.Globalization;
using System.Text;

namespace RoyAppMaui.Extensions;

/// <summary>
/// Extension methods for <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts the string to a UTF-8 byte array.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static byte[] ToBytes(this string str)
    {
        if (str is null)
        {
            throw new ArgumentNullException(nameof(str), "Input string cannot be null.");
        }

        return Encoding.UTF8.GetBytes(str);
    }

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

    public static bool IsFileCsv(this string? filePath) => string.Equals(Path.GetExtension(filePath), ".csv", StringComparison.CurrentCultureIgnoreCase);
}
