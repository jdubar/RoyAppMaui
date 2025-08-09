using CsvHelper.Configuration;

using RoyAppMaui.Converters;
using RoyAppMaui.Models;

namespace RoyAppMaui.ClassMaps;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test class maps. There is no logic to test.")]
public sealed class SleepMap : ClassMap<Sleep>
{
    public SleepMap()
    {
        Map(m => m.Id).Index(0);
        Map(m => m.Bedtime).Index(1).TypeConverter<TimeSpanConverter<TimeSpan>>();
        Map(m => m.Waketime).Index(2).TypeConverter<TimeSpanConverter<TimeSpan>>();
    }
}
