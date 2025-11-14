using CsvHelper.Configuration;

using RoyApp.Converters;
using RoyApp.Models;

namespace RoyApp.ClassMaps;

/// <summary>
/// CsvHelper class map for the Sleep model.
/// Maps Id, Bedtime, and Waketime columns with appropriate type converters.
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test class maps. There is no logic to test.")]
public sealed class SleepMap : ClassMap<Sleep>
{
    /// <summary>
    /// Configures the mapping between Sleep properties and CSV columns.
    /// </summary>
    public SleepMap()
    {
        AutoMap(System.Globalization.CultureInfo.InvariantCulture);

        Map(m => m.Id).Index(0);
        Map(m => m.Bedtime).Index(1).TypeConverter<TimeSpanConverter>();
        Map(m => m.Waketime).Index(2).TypeConverter<TimeSpanConverter>();

        MemberMaps.Where(m => m.Data?.Member?.Name is
                              not (nameof(Sleep.Id)) and
                              not (nameof(Sleep.Bedtime)) and
                              not (nameof(Sleep.Waketime)))
                  .ToList()
                  .ForEach(m => m.Ignore());
    }
}
