using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

using RoyAppMaui.Extensions;

namespace RoyAppMaui.Converters;
public class TimeSpanConverter<T> : DefaultTypeConverter
{
    public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        return string.IsNullOrWhiteSpace(text)
            ? string.Empty
            : text.ToTimeSpan();
    }

    public override string ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
    {
        return value is TimeSpan timeSpan
            ? timeSpan.ToString(@"hh\:mm")
            : string.Empty;
    }
}
