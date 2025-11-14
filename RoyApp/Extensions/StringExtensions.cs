using System.Globalization;
using System.Text;

namespace RoyApp.Extensions;

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

    /// <summary>
    /// Determines whether the specified file path has a ".csv" extension.
    /// </summary>
    /// <remarks>The comparison is performed using the current culture and is case-insensitive.</remarks>
    /// <param name="filePath">The file path to check. This parameter can be <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the file path has a ".csv" extension; otherwise, <see langword="false"/>.</returns>
    public static bool IsFileCsv(this string? filePath) => string.Equals(Path.GetExtension(filePath), ".csv", StringComparison.CurrentCultureIgnoreCase);
}
