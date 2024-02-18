using RoyAppMaui.Models;
using System.Collections.ObjectModel;
using System.Text;

namespace RoyAppMaui.Services.Impl;
public class DataService : IDataService
{
    public decimal GetAverageOfBedtimes(ObservableCollection<Sleep> sleeps) =>
        decimal.Round(sleeps.Sum(s => s.BedtimeRec) / sleeps.Count, 2);

    public decimal GetAverageOfWaketimes(ObservableCollection<Sleep> sleeps) =>
        decimal.Round(sleeps.Sum(s => s.WaketimeRec) / sleeps.Count, 2);

    public decimal GetDuration(decimal bedtime, decimal waketime)
    {
        var duration = waketime - bedtime;
        return duration > 0
                        ? duration
                        : 24 + duration;
    }

    public string GetExportData(ObservableCollection<Sleep> sleeps)
    {
        var sb = new StringBuilder();
        _ = sb.AppendLine("Id,Bedtime,Bedtime (as decimal),Waketime,Waketime (as decimal)");
        foreach (var sleep in sleeps)
        {
            _ = sb.AppendLine($"{sleep.Id},{sleep.BedtimeDisplay},{sleep.BedtimeRec},{sleep.WaketimeDisplay},{sleep.WaketimeRec}");
        }
        _ = sb.AppendLine($"\r\nBedtime Average: {GetAverageOfBedtimes(sleeps)}");
        _ = sb.AppendLine($"Waketime Average: {GetAverageOfWaketimes(sleeps)}");
        return sb.ToString();
    }
}
