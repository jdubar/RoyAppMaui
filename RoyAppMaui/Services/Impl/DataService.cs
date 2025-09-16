using RoyAppMaui.Extensions;
using RoyAppMaui.Models;

using System.Text;

namespace RoyAppMaui.Services.Impl;
public class DataService : IDataService
{
    public byte[] GetExportData(IEnumerable<Sleep> sleeps)
    {
        const string header = "Id,Bedtime,Bedtime (as decimal),Waketime,Waketime (as decimal),Duration";

        var sb = new StringBuilder();
        sb.AppendLine(header);

        if (sleeps.Any())
        {
            foreach (var sleep in sleeps)
            {
                sb.AppendLine($"{sleep.Id},{sleep.BedtimeDisplay},{sleep.BedtimeAsDecimalDisplay},{sleep.WaketimeDisplay},{sleep.WaketimeAsDecimalDisplay},{sleep.DurationDisplay}");
            }
        }

        sb.AppendLine($"Bedtime Average: {sleeps.GetAverage(s => s.BedtimeAsDecimal)}");
        sb.AppendLine($"Waketime Average: {sleeps.GetAverage(s => s.WaketimeAsDecimal)}");
        sb.AppendLine($"Duration Average: {sleeps.GetAverage(s => s.Duration)}");
        return sb.ToString().ToBytes();
    }
}
