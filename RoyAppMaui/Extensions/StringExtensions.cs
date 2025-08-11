using System.Globalization;

namespace RoyAppMaui.Extensions;
public static class StringExtensions
{
    public static byte[] ToBytes(this string str) => System.Text.Encoding.UTF8.GetBytes(str);

    public static TimeSpan ToTimeSpan(this string time)
    {
        string[] formats = ["hhmm", "hmm", @"hh\:mm", @"h\:mm\:ss", @"h:mm", @"h:mm tt", @"HH\:mm", "HHmm"];
        var dateTime = DateTime.ParseExact(time, formats, CultureInfo.InvariantCulture);
        return dateTime.TimeOfDay;
    }
}
