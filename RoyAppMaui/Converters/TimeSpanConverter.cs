using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

using RoyAppMaui.Extensions;

namespace RoyAppMaui.Converters;

/// <summary>
/// CSV type converter for TimeSpan values.
/// </summary>
public class TimeSpanConverter : DefaultTypeConverter
{
    /// <summary>
    /// Converts a CSV string to a TimeSpan. Returns TimeSpan.Zero for empty input.
    /// Throws a FormatException for invalid formats.
    /// </summary>
    public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return TimeSpan.Zero;
        }

        try
        {
            return text.ToTimeSpan();
        }
        catch (FormatException ex)
        {
            throw new TypeConverterException(this, memberMapData, text, row.Context, $"Invalid TimeSpan format: '{text}'.", ex);
        }
    }

    /// <summary>
    /// Converts a TimeSpan to a CSV string in "hh:mm" format.
    /// </summary>
    public override string ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
    {
        return value is TimeSpan timeSpan
            ? timeSpan.ToString(@"hh\:mm", System.Globalization.CultureInfo.InvariantCulture)
            : string.Empty;
    }
}
