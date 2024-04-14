using RoyAppMaui.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace RoyAppMaui.Services.Impl;
public class DataService : IDataService
{
    private readonly CultureInfo _invariant = CultureInfo.InvariantCulture;

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

    public TimeSpan StringToTimeSpan(string time)
    {
        string[] formats = ["hhmm", "hmm", @"hh\:mm", @"h\:mm\:ss", @"h:mm", @"h:mm tt"];
        var dateTime = DateTime.ParseExact(time, formats, _invariant);
        return dateTime.TimeOfDay;
    }

    public string TimeSpanToDateTime(TimeSpan newTime) =>
    DateTime.Today.Add(newTime).ToString("hh:mm tt");

    public decimal TimeSpanToDecimal(TimeSpan? newTime) =>
        decimal.Round(Convert.ToDecimal(TimeSpan.Parse(newTime.ToString() ?? "0:0", _invariant).TotalHours), 2);
}
