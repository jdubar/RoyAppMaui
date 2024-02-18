using RoyAppMaui.Interfaces;
using RoyAppMaui.Models;

using System.Collections.ObjectModel;
using System.Text;

namespace RoyAppMaui.Services;
public class DataService : IDataService
{
    public string GetListDataAsString(ObservableCollection<Sleep> sleeps)
    {
        var sb = new StringBuilder();
        _ = sb.AppendLine("Id,Bedtime,Bedtime (as decimal),Waketime,Waketime (as decimal)");
        foreach (var sleep in sleeps)
        {
            _ = sb.AppendLine($"{sleep.Id},{sleep.BedtimeDisplay},{sleep.BedtimeRec},{sleep.WaketimeDisplay},{sleep.WaketimeRec}");
        }
        _ = sb.AppendLine($"\r\nBedtime Average: {decimal.Round(sleeps.Sum(s => s.BedtimeRec) / sleeps.Count, 2)}");
        _ = sb.AppendLine($"Waketime Average: {decimal.Round(sleeps.Sum(s => s.WaketimeRec) / sleeps.Count, 2)}");
        return sb.ToString();
    }
}
