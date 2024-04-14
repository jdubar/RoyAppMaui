using RoyAppMaui.Models;
using System.Collections.ObjectModel;

namespace RoyAppMaui.Services;
public interface IDataService
{
    decimal GetAverageOfBedtimes(ObservableCollection<Sleep> sleeps);
    decimal GetAverageOfWaketimes(ObservableCollection<Sleep> sleeps);
    decimal GetDuration(decimal bedtime, decimal waketime);
    string GetExportData(ObservableCollection<Sleep> sleeps);
    TimeSpan StringToTimeSpan(string time);
    string TimeSpanToDateTime(TimeSpan newTime);
    decimal TimeSpanToDecimal(TimeSpan? newTime);

}
